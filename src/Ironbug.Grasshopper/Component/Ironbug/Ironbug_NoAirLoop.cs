using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_NoAirLoop : Ironbug_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_AirLoopHVAC class.
        /// </summary>
        public Ironbug_NoAirLoop()
          : base("Ironbug_NoAirLoop", "NoAirLoop",
              "An void loop for zones don't need an airloop, but only with ZoneEquipments",
              "Ironbug", "01:Loops")
        {
        }

        protected override System.Drawing.Bitmap Icon => Resources.NoAirLoop;

        public override Guid ComponentGuid => new Guid("31ED5059-5198-4099-9B70-4C7A86D9E27C");

        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("OSZones", "OSZones", "ThermalZones without airloops", GH_ParamAccess.list);
            pManager[0].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("NoAirLoop", "NoAirLoop", "To HVACsystem", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
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