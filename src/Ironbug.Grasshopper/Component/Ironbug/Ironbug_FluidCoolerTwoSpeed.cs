using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_FluidCoolerTwoSpeed : Ironbug_DuplicableHVACWithParamComponent
    {
        
        
        /// Initializes a new instance of the Ironbug_BoilerHotWater class.
        
        public Ironbug_FluidCoolerTwoSpeed()
          : base("Ironbug_FluidCoolerTwoSpeed", "FluidCooler2",
              "Description",
              "Ironbug", "02:LoopComponents", 
              typeof(HVAC.IB_FluidCoolerTwoSpeed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FluidCoolerTwoSpeed", "FluidCooler2", "FluidCoolerTwoSpeed", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_FluidCoolerTwoSpeed();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.FluidCooler2;

        public override Guid ComponentGuid => new Guid("{FA65DD5E-1ACA-4271-B116-EB3B5E7792C4}");
    }
}