using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HeatExchangerFluidToFluid_Obsolete : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_HeatExchangerFluidToFluid_Obsolete()
          : base("IB_HeatExchangerFluidToFluid_Obsolete", "HeatExchangerFluid",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_HeatExchangerFluidToFluid_FieldSet))
        {
        }
        public override bool Obsolete => true;
    
        protected override System.Drawing.Bitmap Icon => Properties.Resources.HXFluid;
        public override Guid ComponentGuid => new Guid("6A078A99-218A-449D-9E5F-9F8B6E86A0D6");
        public override GH_Exposure Exposure => GH_Exposure.hidden;

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

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }



    }

   
}