using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ironbug.HVAC
{
    public class IB_Utility
    {
        private static List<Func<bool>> _delaiedAMFuncs;

        /// <summary>
        /// Collect Funcs that need to be executed after all loops are saved.
        /// </summary>
        /// <param name="func"></param>
        public static void DelayAddSensorNode(Func<bool> func)
        {
            // This can only be called when saving the HVAC system to the OpenStudio model.
            if (_delaiedAMFuncs == null)
                return;

            _delaiedAMFuncs.Add(func);
        }

        private static void ExecuteDelayedFuns()
        {
            if (_delaiedAMFuncs == null)
                return;
            foreach (var f in _delaiedAMFuncs)
            {
                if (!(f?.Invoke()).GetValueOrDefault())
                    throw new ArgumentException("Failed to set availability manager's node"); 
            }

            // clear
            _delaiedAMFuncs = null;
        }

        private static void StartSaving()
        {
            OpsIDMapper.StartRecording();

            _delaiedAMFuncs = new();
        }



        public static bool SaveHVAC(IB_HVACSystem hvac, string osmFile)
        {
            StartSaving();

            var airLoops = hvac.AirLoops;
            var plantLoops = hvac.PlantLoops;
            var vrfs = hvac.VariableRefrigerantFlows;


            //get Model from file if exists
            var model = GetOrNewModel(osmFile);

            //Add outdoor air temperature output variable
            var outT = new OpenStudio.OutputVariable("Site Outdoor Air Drybulb Temperature", model);
            outT.setReportingFrequency("Hourly");

            //add loops
            //added plantLoops first, as the controllerWaterCoil of CoilCoolingWater or CoilHeatingWater only exists after the coil is added to PlantLoop
            // add Condenser water loop before chilled water loop so that the chiller's condenser type can be set to WaterCooled
            //var condenserLps = plantLoops.OfType<IB_CondenserPlantLoop>();
            //foreach (var cdPlant in condenserLps)
            //{
            //    cdPlant.ToOS(model);
            //}
            //var theRestLps = plantLoops.Where(_ => !(_ is IB_CondenserPlantLoop));
            foreach (var plant in plantLoops)
            {
                plant.ToOS(model);
            }
            //don't add airloop before the plantLoops
            foreach (var airLoop in airLoops)
            {
                airLoop.ToOS(model);
            }

            foreach (var vrf in vrfs)
            {
                vrf.ToOS(model);
            }

            // set availability manager's sensor nodes after all loops are saved
            ExecuteDelayedFuns();


            CheckInternalSourceConstruction(model);
            var tol = model.getOutputControlReportingTolerances();
            tol.setToleranceforTimeCoolingSetpointNotMet(1.11);
            tol.setToleranceforTimeHeatingSetpointNotMet(1.11);

            //save workflow 
            var osw = Path.Combine(Path.GetDirectoryName(osmFile), Path.GetFileNameWithoutExtension(osmFile), "workflow.osw");
            var wf = model.workflowJSON();
            wf.setSeedFile(OpenStudio.OpenStudioUtilitiesCore.toPath(Path.Combine("..", Path.GetFileName(osmFile))));
            wf.saveAs(OpenStudio.OpenStudioUtilitiesCore.toPath(osw));
            if (!File.Exists(osw))
                throw new ArgumentException($"Failed to create workflowJSON file: {osw}");


            //save osm file
            return model.Save(osmFile);
        }

        public static OpenStudio.Model GetOrNewModel(string opsFilePath)
        {
            OpenStudio.Model model = null;
            var f = new FileInfo(opsFilePath);

            if (f.Exists &&
                f.Length > 0 &&
                f.Extension.ToLower().Equals(".osm"))
            {
                //var osmPath = opsModelFilePath.ToPath();
                //CheckIfOldVersion(osmPath);

                // works
                var vt = new OpenStudio.VersionTranslator();
                vt.setAllowNewerVersions(false);
                var optionalModel = vt.loadModel(OpenStudio.OpenStudioUtilitiesCore.toPath(opsFilePath));

                if (optionalModel.is_initialized())
                {
                    //var warnings = vTranslator.warnings().Select(_ => _.logMessage()).ToList();
                    var errors = vt.errors().Select(_ => _.logMessage()).ToList();
                    if (errors.Any())
                        throw new ArgumentException($"Failed to load OpenStudio Model from {opsFilePath} because the following errors:{Environment.NewLine}{string.Join(Environment.NewLine, errors)}");

                    model = optionalModel.get();

                }
                else
                    throw new ArgumentException($"Failed to load OpenStudio Model from {opsFilePath}");

                if (!model.isValid())
                    throw new ArgumentException($"Found an invalid OpenStudio Model from {opsFilePath}");

            }
            else
            {
                model = new OpenStudio.Model();
            }
            return model;
        }


        //This is due to how HB sets up the this type of construction 
        private static void CheckInternalSourceConstruction(OpenStudio.Model model)
        {
            var tempM_o = model.getMaterialByName("INTERNAL SOURCE");
            if (tempM_o.isNull()) return;
            var tempM = tempM_o.get();
            var handle = tempM.handle().__str__();

            var tempCName = string.Empty;

            foreach (var item in tempM.sources())
            {
                if (item.to_Construction().isNull()) continue;

                var tempC = item.to_Construction().get();
                tempCName = tempC.nameString();
                var mLayers = tempC.layers();

                var newLayers = new OpenStudio.MaterialVector(mLayers);
                var index = newLayers.ToList().FindIndex(_ => _.nameString() == "INTERNAL SOURCE");
                newLayers.RemoveAt(index);

                //create ConstructionWithInternalSource
                var opC = new OpenStudio.ConstructionWithInternalSource(model);
                opC.setName(tempCName);
                opC.setLayers(newLayers);
                opC.setSourcePresentAfterLayerNumber(index);
                var opC_rev = opC.reverseConstructionWithInternalSource();
                var opCname = opC.nameString();

                //assign to surfaces and their adjacent surfaces
                var surfacesWithConstr = tempC.sources().Where(_ => _.to_Surface().is_initialized()).Select(_ => _.to_Surface().get());
                foreach (var srf in surfacesWithConstr)
                {
                    srf.setConstruction(opC);
                    if (srf.adjacentSurface().is_initialized())
                    {
                        srf.adjacentSurface().get().setConstruction(opC_rev);
                    }
                }

                //in case that construction is set to default construction set
                var defconstrs = tempC.sources().Where(_ => _.to_DefaultSurfaceConstructions().is_initialized()).Select(_ => _.to_DefaultSurfaceConstructions().get());
                //var names = defconstrs.SelectMany(_ => _.children().Select(c => c.nameString()));
                foreach (var defcon in defconstrs)
                {
                    if (defcon.wallConstruction().is_initialized())
                    {
                        if (defcon.wallConstruction().get().nameString() == tempCName)
                        {
                            defcon.setWallConstruction(opC);
                        }

                    }

                    if (defcon.floorConstruction().is_initialized())
                    {
                        if (defcon.floorConstruction().get().nameString() == tempCName)
                        {
                            defcon.setFloorConstruction(opC);
                        }
                    }

                    if (defcon.roofCeilingConstruction().is_initialized())
                    {
                        if (defcon.roofCeilingConstruction().get().nameString() == tempCName)
                        {
                            defcon.setRoofCeilingConstruction(opC);
                        }
                    }


                }
                tempC.remove();
            }


        }

    }
}
