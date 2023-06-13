using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Ironbug.Core;
using Ironbug.HVAC.BaseClass;
using System.Dynamic;

namespace Ironbug.HVAC
{
    public class IB_HVACSystem
    {
        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public List<IB_AirLoopHVAC> AirLoops { get; private set; }
        [DataMember]
        public List<IB_PlantLoop> PlantLoops { get; private set; }
        [DataMember]
        public List<IB_AirConditionerVariableRefrigerantFlow> VariableRefrigerantFlows { get; private set; }

        private static string _version;
        [DataMember]
        public string IBVersion
        {
            get
            {
                _version = _version?? typeof(IB_HVACSystem).Assembly.GetName().Version.ToString();
                return _version;
            }
            private set => _version = value;
        }

        [IgnoreDataMember]
        private string _existFile = "";

        private IB_HVACSystem() 
        {

            this.AirLoops = new List<IB_AirLoopHVAC>();
            this.PlantLoops = new List<IB_PlantLoop>();
            this.VariableRefrigerantFlows = new List<IB_AirConditionerVariableRefrigerantFlow>();
        }
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
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(this, format, IB_JsonSetting.ConvertSetting);
            var dup = IB_HVACSystem.FromJson(json);
            if (this != dup)
                throw new ArgumentException("Failed to serialize IB_HVACSystem, please report this to the developer!");
            return json;

        }

        public object ToExpandoObject()
        {
            var json = ToJson();
            return JsonConvert.DeserializeObject<ExpandoObject>(json);
        }

        public string SaveAsJson(string path)
        {
            var json = this.ToJson();
            File.WriteAllText(path, json);
            return path;
        }

        public static IB_HVACSystem FromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new ArgumentNullException("json","Failed to deserialize the JSON to HVACSystem");
            try
            {
                var hvac = JsonConvert.DeserializeObject<IB_HVACSystem>(json, IB_JsonSetting.ConvertSetting);
                return hvac;
            }
            catch (Exception e)
            {
                throw e;
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

        public static bool SaveHVAC(string osmPath, string hvacJsonFilePath)
        {
            var osm = osmPath;
            var hvac = hvacJsonFilePath;

            if (!System.IO.File.Exists(osm) || !osm.ToLower().EndsWith(".osm"))
                throw new ArgumentException($"Invalid osm file: {osm}");
            if (!System.IO.File.Exists(hvac))
                throw new ArgumentException($"Invalid HVAC file: {hvac}");

            var hvacJson = System.IO.File.ReadAllText(hvac);
            var system = Ironbug.HVAC.IB_HVACSystem.FromJson(hvacJson);

            if (system == null)
                throw new ArgumentException("Invalid HVAC");

            var done = system.SaveHVAC(osm);
            return done;

        }

        public bool SaveHVAC(string osmFile)
        {
            var airLoops = this.AirLoops;
            var plantLoops = this.PlantLoops;
            var vrfs = this.VariableRefrigerantFlows;


            //here means editing current existing file 
            if (!string.IsNullOrEmpty( this._existFile))
            {
                if (this._existFile != osmFile)
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
            var f= new FileInfo(opsFilePath);
            
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

        public override string ToString()
        {
            var info = new List<string>();
            var s = $"HVAC System: [{this.DisplayName ?? "Unnamed"}]";
            if (AirLoops.Any())
            {
                var loops = this.AirLoops.GroupBy(_ => _ is HVAC.IB_NoAirLoop);
                var noAir = loops.FirstOrDefault(_ => _.Key == true)?.OfType<HVAC.IB_NoAirLoop>();
                if (noAir != null)
                    s = $"{s}{Environment.NewLine}  - Zonal-system: {noAir.SelectMany(_=>_.ThermalZones).Count()}";
                var airCount = loops.FirstOrDefault(_ => _.Key == false)?.Count();
                if (airCount.HasValue)
                    s = $"{s}{Environment.NewLine}  - Air loop: {airCount.GetValueOrDefault()}";
            }

            if (PlantLoops.Any())
            {
                s = $"{s}{Environment.NewLine}  - Plant loop: {PlantLoops.Count}";
            }

            if (VariableRefrigerantFlows.Any())
            {
                s = $"{s}{Environment.NewLine}  - VRF system: {VariableRefrigerantFlows.Count}";
            }

            // zone names
            var sys = this;
            var rooms = sys.GetThermalZoneNames().Select((_, i) => $"{i + 1}: [{_}]").ToList();
            if (!rooms.Any()) rooms.Add("No zone is assigned to this system!");
            else
            {
                var zoneNames = $"- Zone names: {rooms.Count}";
                s = $"{s}{Environment.NewLine}  - VRF system: {VariableRefrigerantFlows.Count}";
                info.Add(zoneNames.PadLeft(zoneNames.Length + 2));
            }
            if (rooms.Count > 3)
            {
                rooms = rooms.Take(3).ToList();
                rooms.Add("...");
            }
            rooms = rooms.Select(_ => _.PadLeft(_.Length + 4)).ToList();

            info.AddRange(rooms);
            info.Insert(0, s);
            s = string.Join(Environment.NewLine, info);
            s = $"{s}{Environment.NewLine}";
            return s;
        }

        public List<IB_ThermalZone> GetThermalZones()
        {
            return this.AirLoops?
                .SelectMany(_ => _.GetThermalZones())?
                .ToList() ?? new List<IB_ThermalZone>();
        }

        public List<string> GetThermalZoneNames()
        {
            return this.GetThermalZones()?
                .Select(_ => _?.ZoneName)?
                .Where(_=> !string.IsNullOrEmpty(_))?
                .ToList() ?? new List<string>();

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
            if (!same) return false;
            same &= this.PlantLoops.SequenceEqual(other.PlantLoops);
            if (!same) return false;
            same &= this.VariableRefrigerantFlows.SequenceEqual(other.VariableRefrigerantFlows); 
            if (!same) return false;
            same &= this.GetType() == other.GetType();
            if (!same) return false;
            same &= this.IBVersion== other.IBVersion;
            return same;
        }

        public override int GetHashCode()
        {
            int hashCode = 1811681192;
            hashCode = hashCode * -1521134295 + EqualityComparer<List<IB_AirLoopHVAC>>.Default.GetHashCode(AirLoops);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<IB_PlantLoop>>.Default.GetHashCode(PlantLoops);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<IB_AirConditionerVariableRefrigerantFlow>>.Default.GetHashCode(VariableRefrigerantFlows);
            return hashCode;
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
