using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ExistAirLoopHVAC : Ironbug_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_AirLoopHVAC class.
        /// </summary>
        public Ironbug_ExistAirLoopHVAC()
          : base("Ironbug_AirLoopHVAC", "ExistAirLoop",
              EPDoc.AirLoopHVAC.Note,
              "Ironbug", "01:Loops")
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("OsAirLoop", "OsAirLoop", "The existing air loop from Ironbug_ImportOSM component", GH_ParamAccess.item);
            pManager.AddGenericParameter("Thermal Zones", "zones", "Zones to be added to new branches of this existing loop", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirLoopHVAC", "AirLoop", "toSaveOSM", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IB_ExistingObj name = null;
            DA.GetData(0, ref name);

            var demandComs = new List<IB_ThermalZone>();
            DA.GetDataList(1, demandComs);
           
            var airLoop = new HVAC.IB_ExistAirLoop(name);
            
            foreach (var item in demandComs)
            {
                airLoop.AddThermalZones(item);
            }
            
            
            DA.SetData(0, airLoop);

        }
        
        protected override System.Drawing.Bitmap Icon => Resources.AirLoop_Exist;
        public override Guid ComponentGuid
        {
            get { return new Guid("1263A5C1-4D65-4C27-9821-82C118702331"); }
        }
        
    }
}