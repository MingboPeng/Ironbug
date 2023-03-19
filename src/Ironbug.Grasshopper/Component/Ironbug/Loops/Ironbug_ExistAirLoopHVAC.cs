using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ExistAirLoopHVAC : Ironbug_Component
    {
        
        public Ironbug_ExistAirLoopHVAC()
          : base("IB_ExistingAirLoop", "ExistingAirLoop",
              HVAC.IB_AirLoopHVAC_FieldSet.Value.OwnerEpNote,
              "Ironbug", "01:Loops")
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;


        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("OsAirLoop", "OsAirLoop", "The existing air loop from Ironbug_ImportOSM component", GH_ParamAccess.item);
            pManager.AddGenericParameter("Thermal Zones", "zones", "Zones to be added to new branches of this existing loop", GH_ParamAccess.list);
        }

        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("AirLoopHVAC", "AirLoop", "toSaveOSM", GH_ParamAccess.item);
        }

        
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
        public override Guid ComponentGuid => new Guid("1263A5C1-4D65-4C27-9821-82C118702331");

    }
}