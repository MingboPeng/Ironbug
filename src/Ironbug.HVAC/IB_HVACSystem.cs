using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ironbug.HVAC
{
    public class IB_HVACSystem
    {
        public List<IB_AirLoopHVAC> AirLoops { get; private set; }
        public List<IB_PlantLoop> PlantLoops { get; private set; }
        public List<IB_AirConditionerVariableRefrigerantFlow> VariableRefrigerantFlows { get; private set; }

        private string _existFile = "";

        public IB_HVACSystem(List<IB_AirLoopHVAC> airLoops, List<IB_PlantLoop> plantLoops, List<IB_AirConditionerVariableRefrigerantFlow> vrfs)
        {
            this.AirLoops = airLoops;
            this.PlantLoops = plantLoops;
            this.VariableRefrigerantFlows = vrfs;
            
            var existingA = airLoops.Where(_=>_ is IIB_ExistingLoop).Select(_=>((IIB_ExistingLoop)_).ExistingObj.OsmFile);
            var existingP = plantLoops.Where(_ => _ is IIB_ExistingLoop).Select(_ => ((IIB_ExistingLoop)_).ExistingObj.OsmFile);

            var existing = existingA.ToList();
            existing.AddRange(existingP);
            existing.Distinct();
            if (existing.Count >1)
            {
                throw new ArgumentException("Cannot merge loops from different osm files");
            }
            else if (existing.Count==1)
            {
                _existFile = existing[0];
            }

        }

        /// <summary>
        /// Save the model to a default temp file in temp folder
        /// </summary>
        /// <returns>A full osm model file path</returns>
        public string Save2Temp()
        {
            var tempPath = Path.GetTempPath() + @"\Ladybug\HVAC";
            Directory.CreateDirectory(tempPath);

            tempPath = $"{tempPath}\\temp.osm";

            return this.SaveHVAC(tempPath) ? tempPath : string.Empty;
            
        }

        public bool SaveHVAC(string filepath)
        {
            var airLoops = this.AirLoops;
            var plantLoops = this.PlantLoops;
            var vrfs = this.VariableRefrigerantFlows;

            var osmFile = filepath;

            //here means editing current existing file 
            if (!string.IsNullOrEmpty( this._existFile))
            {
                if (this._existFile != filepath)
                {
                    throw new ArgumentException("File path is different than osm file contains existing loops!\nPlease input the existing osm file path.");
                }
                //osmFile = this._existFile;
            }

            //get Model from file if exists
            var model = GetOrNewModel(osmFile);

            //Add outdoor air temperature output variable
            var outT = new OpenStudio.OutputVariable("Site Outdoor Air Drybulb Temperature", model);
            outT.setReportingFrequency("Hourly");

            //add loops
            //added plantLoops first, as the controllerWaterCoil of CoilCoolingWater or CoilHeatingWater only exists after the coil is added to PlantLoop
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

            CheckInternalSourceConstruction(model);
            var tol = model.getOutputControlReportingTolerances();
            tol.setToleranceforTimeCoolingSetpointNotMet(1.11);
            tol.setToleranceforTimeHeatingSetpointNotMet(1.11);

            //save workflow 
            var osw = Path.Combine(Path.GetDirectoryName(filepath), Path.GetFileNameWithoutExtension(filepath), "workflow.osw");
            var wf = model.workflowJSON();
            wf.setSeedFile(OpenStudio.OpenStudioUtilitiesCore.toPath(Path.Combine("..", Path.GetFileName(filepath))));
            wf.saveAs(OpenStudio.OpenStudioUtilitiesCore.toPath(osw));
            if (!File.Exists(osw))
                throw new ArgumentException($"Failed to create workflowJSON file: {osw}");


            //save osm file
            var osmPath = OpenStudio.OpenStudioUtilitiesCore.toPath(filepath);
            return model.save(osmPath, true);
            
        }

        public static OpenStudio.Model GetOrNewModel(string opsModelFilePath)
        {
            var model =  new OpenStudio.Model();
            if (File.Exists(opsModelFilePath))
            {
                var osmPath = opsModelFilePath.ToPath();
                CheckIfOldVersion(osmPath);
                var optionalModel = OpenStudio.Model.load(osmPath);

                if(optionalModel.is_initialized()) model = optionalModel.get();

            }
            return model;

            bool CheckIfOldVersion(OpenStudio.Path p)
            {
                var ts = new OpenStudio.VersionTranslator();
                var m = ts.loadModel(p).get();
                var v1 = ts.originalVersion().str();
                var v2 = m.version().str();
                if (v1 != v2)
                    throw new ArgumentException($"Incompatible OpenStudio file version {v1} which is different than what Ironbug is using ({v2})");
                return true;
            }
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
                var defconstrs = tempC.sources().Where(_=> _.to_DefaultSurfaceConstructions().is_initialized()).Select(_ => _.to_DefaultSurfaceConstructions().get());
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
