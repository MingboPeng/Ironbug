using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HeatExchangerFluidToFluid : Ironbug_HVACComponent
    {
        public Ironbug_HeatExchangerFluidToFluid()
          : base("Ironbug_HeatExchangerFluidToFluid", "HeatExchangerFluid",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_HeatExchangerFluidToFluid_FieldSet))
        {
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.HXFluid;
        public override Guid ComponentGuid => new Guid("6A078A99-218A-449D-9E5F-9F8B6E86A0D6");
        public override GH_Exposure Exposure => GH_Exposure.quarternary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatExchangerFluidToFluid", "HXFluid", "HeatExchangerFluidToFluid", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_HeatExchangerFluidToFluid();

            var objs = this.SetObjParamsTo(obj);
            DA.SetDataList(0, objs);
        }



    }

   
}