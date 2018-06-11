using System;
using System.Collections.Generic;
using System.IO;

namespace Ironbug.HVAC
{
    public class IB_HVACSystem
    {
        public List<IB_AirLoopHVAC> AirLoops { get; private set; }
        public List<IB_PlantLoop> PlantLoops { get; private set; }
        public List<IB_AirConditionerVariableRefrigerantFlow> VariableRefrigerantFlows { get; private set; }

        public IB_HVACSystem(List<IB_AirLoopHVAC> airLoops, List<IB_PlantLoop> plantLoops, List<IB_AirConditionerVariableRefrigerantFlow> vrfs)
        {
            this.AirLoops = airLoops;
            this.PlantLoops = plantLoops;
            this.VariableRefrigerantFlows = vrfs;
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

            var osmPath = new OpenStudio.Path(filepath);
            //get Model from file if exists
            var model = GetOrNewModel(filepath);
            
            
            //add loops
            foreach (var airLoop in airLoops)
            {
                airLoop.ToOS(model);
            }

            foreach (var plant in plantLoops)
            {
                plant.ToOS(model);
            }

            foreach (var vrf in vrfs)
            {
                vrf.ToOS(model);
            }
            
            //save osm file
            return model.save(osmPath, true);
            
        }

        private static OpenStudio.Model GetOrNewModel(string opsModelFilePath)
        {
            if (!File.Exists(opsModelFilePath)) throw new Exception("OSM file doesn't exist!");
            
            var osmPath = new OpenStudio.Path(opsModelFilePath);

            var optionalModel = OpenStudio.Model.load(osmPath);
            var model = optionalModel.is_initialized() ? optionalModel.get() : new OpenStudio.Model();

            return model;
        }


        //private void SetThermalZones(OpenStudio.Model model)
        //{
        //    //link ThermalZone to Space(HBZone)
        //    var thermalZones = model.getThermalZones();
        //    var spaces = model.getSpaces();

        //    foreach (var space in spaces)
        //    {
        //        //TODO: check what if couldn't find it by name??
        //        var name = space.nameString().Replace("_space", "");
        //        var thermalZone = thermalZones.Where(_ => _.nameString() == name).First();
        //        space.setThermalZone(thermalZone);
        //    }
        //}

    }
}
