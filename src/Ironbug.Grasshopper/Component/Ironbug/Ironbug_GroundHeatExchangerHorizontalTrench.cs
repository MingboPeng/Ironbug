using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_GroundHeatExchangerHorizontalTrench : Ironbug_DuplicatableHVACWithParamComponent
    {
        
        public Ironbug_GroundHeatExchangerHorizontalTrench()
          : base("Ironbug_GroundHeatExchangerHorizontalTrench", "GroundHXHorizontal",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_GroundHeatExchangerHorizontalTrench_FieldSet))
        {
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GSHPH;
        public override Guid ComponentGuid => new Guid("CDB436A2-F50E-4001-B3C1-50A3078ABE36");
        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("GroundHeatExchangerHorizontalTrench", "GroundHX", "GroundHeatExchangerHorizontalTrench", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_GroundHeatExchangerHorizontalTrench();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }



    }

   
}