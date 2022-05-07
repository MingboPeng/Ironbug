using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_EvaporativeFluidCoolerSingleSpeed : Ironbug_DuplicableHVACWithParamComponent
    {
        
        
        /// Initializes a new instance of the Ironbug_BoilerHotWater class.
        
        public Ironbug_EvaporativeFluidCoolerSingleSpeed()
          : base("IB_EvaporativeFluidCoolerSingleSpeed", "EvapFluidCooler1",
              "Description",
              "Ironbug", "02:LoopComponents", 
              typeof(HVAC.IB_EvaporativeFluidCoolerSingleSpeed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("EvaporativeFluidCoolerSingleSpeed", "FluidCooler1", "EvaporativeFluidCoolerSingleSpeed", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_EvaporativeFluidCoolerSingleSpeed();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.FluidCooler1;

        public override Guid ComponentGuid => new Guid("{416C60D1-7A1C-4253-B8CE-1AD383C4051C}");
    }
}