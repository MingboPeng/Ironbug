using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_NoAirLoop : Ironbug_Component
    {
        public Ironbug_NoAirLoop()
          : base("Ironbug_NoAirLoop", "NoAirLoop",
              "An void loop for zones don't need an airloop, but only with ZoneEquipments",
              "Ironbug", "01:Loops")
        {
        }

        protected override System.Drawing.Bitmap Icon => Resources.NoAirLoop;

        public override Guid ComponentGuid => new Guid("31ED5059-5198-4099-9B70-4C7A86D9E27C");

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("OSZones", "Zones", "ThermalZones that only have zone equipments. No airloop will be added", GH_ParamAccess.list);
            //pManager[0].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("NoAirLoop", "NoAirLoop", "To HVACsystem", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var zones = new List<IB_ThermalZone>();
            DA.GetDataList(0, zones);
            
            var airLoop = new HVAC.IB_NoAirLoop();

            //TODO: need to check nulls
            foreach (var item in zones)
            {
                airLoop.AddThermalZones(item);
            }
            
            DA.SetData(0, airLoop);

        }
        

    }
}