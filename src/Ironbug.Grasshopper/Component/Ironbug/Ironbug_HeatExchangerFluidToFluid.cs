using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HeatExchangerFluidToFluid : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_HeatExchangerFluidToFluid()
          : base("IB_HeatExchangerFluidToFluid", "HeatExchangerFluid",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_HeatExchangerFluidToFluid_FieldSet))
        {
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.HXFluid;
        public override Guid ComponentGuid => new Guid("{B2640599-4B0B-4B47-A3F7-4A7C83C4E0FE}");
        public override GH_Exposure Exposure => GH_Exposure.quarternary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("HeatExchangerFluidToFluid", "AtDemand", "HeatExchangerFluidToFluid connects to source plantloop demand side.", GH_ParamAccess.item);
            pManager[pManager.AddGenericParameter("HeatExchangerFluidToFluid", "AtSupply", "HeatExchangerFluidToFluid connects to another plantloop supply side.", GH_ParamAccess.item)].DataMapping = GH_DataMapping.Graft;
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_HeatExchangerFluidToFluid();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
            DA.SetDataList(1, objs);
        }



    }

   
}