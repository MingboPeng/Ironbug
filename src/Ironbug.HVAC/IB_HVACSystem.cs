using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Ironbug.Core;

namespace Ironbug.HVAC
{
    public class IB_HVACSystem
    {
        [DataMember]
        public List<IB_AirLoopHVAC> AirLoops { get; private set; }
        [DataMember]
        public List<IB_PlantLoop> PlantLoops { get; private set; }
        [DataMember]
        public List<IB_AirConditionerVariableRefrigerantFlow> VariableRefrigerantFlows { get; private set; }

        private string _existFile = "";

        public IB_HVACSystem(List<IB_AirLoopHVAC> airLoops, List<IB_PlantLoop> plantLoops, List<IB_AirConditionerVariableRefrigerantFlow> vrfs)
        {
            airLoops = airLoops ?? new List<IB_AirLoopHVAC>();
            plantLoops = plantLoops ?? new List<IB_PlantLoop>();
            vrfs = vrfs ?? new List<IB_AirConditionerVariableRefrigerantFlow>();

            this.AirLoops = airLoops;
            this.PlantLoops = plantLoops;
            this.VariableRefrigerantFlows = vrfs;
            
            var existingA = this.AirLoops.Where(_=>_ is IIB_ExistingLoop).Select(_=>((IIB_ExistingLoop)_).ExistingObj.OsmFile);
            var existingP = this.PlantLoops.Where(_ => _ is IIB_ExistingLoop).Select(_ => ((IIB_ExistingLoop)_).ExistingObj.OsmFile);

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

        #region Serialization
        public bool ShouldSerializeAirLoops() => !this.AirLoops.IsNullOrEmpty();
        public bool ShouldSerializePlantLoops() => !this.PlantLoops.IsNullOrEmpty();
        public bool ShouldSerializeVariableRefrigerantFlows() => !this.VariableRefrigerantFlows.IsNullOrEmpty();
        #endregion

        public string ToJson(bool indented = false)
        {
            var format = indented ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None;
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, format, IB_JsonSetting.ConvertSetting);
        }

        public string SaveAsIBJson(string path)
        {
            var json = this.ToJson();
          
            using (StreamWriter file = new StreamWriter(path, true))
            {
                file.Write(json);
            }
            return path;
        }

        public static IB_HVACSystem FromJson(string json)
        {
            var hvac = JsonConvert.DeserializeObject<IB_HVACSystem>(json, IB_JsonSetting.ConvertSetting);
            return hvac;
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


        public override bool Equals(object obj)
        {
            return this.Equals(obj as IB_HVACSystem);
        }

        public bool Equals(IB_HVACSystem other)
        {
            if (other is null)
                return this is null ? true : false;
            var same = this.AirLoops.SequenceEqual(other.AirLoops);
            same &= this.PlantLoops.SequenceEqual(other.PlantLoops);
            same &= this.VariableRefrigerantFlows.SequenceEqual(other.VariableRefrigerantFlows);
            same &= this.GetType() == other.GetType();
            return same;
        }
        public static bool operator ==(IB_HVACSystem x, IB_HVACSystem y)
        {
            if (x is null)
                return y is null ? true : false;
            return x.Equals(y);
        }

        public static bool operator !=(IB_HVACSystem x, IB_HVACSystem y) => !(x == y);
    }
}
