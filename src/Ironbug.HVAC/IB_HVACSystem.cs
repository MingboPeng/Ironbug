using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public bool Save(string filepath)
        {
            var airLoops = this.AirLoops;
            var plantLoops = this.PlantLoops;
            var vrfs = this.VariableRefrigerantFlows;

            var osmPath = new OpenStudio.Path(filepath);
            //get Model from file if exists
            var model = new OpenStudio.Model();
            //if (File.Exists(filepath))
            //{
            //    model.Dispose();

            //    var optionalModel = OpenStudio.Model.load(osmPath);
            //    model = optionalModel.is_initialized() ? optionalModel.get() : model;
            //}

            ////remove current thermalzones
            ////TODO:need to duplicate thermalzone's schedules, etc.
            //var currThermalZones = model.getThermalZones();
            //foreach (var item in currThermalZones)
            //{
            //    //TODO: memory protected if path connected before airloop
            //    item.disconnect();
            //    //item.remove();
            //    //item.Dispose();


            //}

            //var currentAirloop = model.getAirLoopHVACs();
            //foreach (var item in currentAirloop)
            //{
            //    item.remove();
            //}

            //var currentPlantloop = model.getPlantLoops();
            //foreach (var item in currentPlantloop)
            //{
            //    item.remove();
            //}

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

            //this.SetThermalZones(model);

            //save osm file
            
            return model.save(osmPath, true);
            
        }



        private void SetThermalZones(OpenStudio.Model model)
        {
            //link ThermalZone to Space(HBZone)
            var thermalZones = model.getThermalZones();
            var spaces = model.getSpaces();


            //if (thermalZones.Count != spaces.Count)
            //{
            //    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "It seems not all HBZones have HVAC set correctly!");

            //}

            foreach (var space in spaces)
            {
                //TODO: check what if couldn't find it by name??
                var name = space.nameString().Replace("_space", "");
                var thermalZone = thermalZones.Where(_ => _.nameString() == name).First();
                space.setThermalZone(thermalZone);
            }
        }

    }
}
