using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HVACSystem : Ironbug_Component
    {
        public Ironbug_HVACSystem()
          : base("IB_HVACSystem", "HVACSystem",
              "Description",
              "Ironbug", "HVAC")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("AirLoops", "AirLoops", "AirLoops", GH_ParamAccess.list);
            pManager.AddGenericParameter("PlantLoops", "PlantLoops", "PlantLoops", GH_ParamAccess.list);
            pManager.AddGenericParameter("VRFSystems", "VRFSystems", "VRFSystems", GH_ParamAccess.list);

            pManager[0].Optional = true;
            pManager[0].DataMapping = GH_DataMapping.Flatten;
            pManager[1].Optional = true;
            pManager[1].DataMapping = GH_DataMapping.Flatten;
            pManager[2].Optional = true;
            pManager[2].DataMapping = GH_DataMapping.Flatten;

        }
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("HVACSystem", "HVACSystem", "A fully detailed HVAC system for ExportToOpenStudio component.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var airLoops = new List<HVAC.IB_AirLoopHVAC>();
            var plantLoops = new List<HVAC.IB_PlantLoop>();
            var vrfs = new List<HVAC.IB_AirConditionerVariableRefrigerantFlow>();

            DA.GetDataList(0, airLoops);
            DA.GetDataList(1, plantLoops);
            DA.GetDataList(2, vrfs);
            
            var hvac = new HVAC.IB_HVACSystem(airLoops, plantLoops, vrfs);
            DA.SetData(0, hvac);


        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.HVAC;

        public override Guid ComponentGuid => new Guid("330C6DCC-EC73-49C7-96CB-B0EB522A1585");


    }
}