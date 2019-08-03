using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_BoilerHotWater : Ironbug_DuplicatableHVACComponent
    {
        
        public Ironbug_BoilerHotWater()
          : base("Ironbug_BoilerHotWater", "Boiler",
              "Description",
              "Ironbug", "02:LoopComponents", 
              typeof(HVAC.IB_BoilerHotWater_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BoilerHotWater", "Boiler", "connect to plantloop's supply side", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_BoilerHotWater();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Resources.Boiler;

        public override Guid ComponentGuid => new Guid("5281d0b8-492e-4c52-a372-d9a63a94b4df");
    }
}