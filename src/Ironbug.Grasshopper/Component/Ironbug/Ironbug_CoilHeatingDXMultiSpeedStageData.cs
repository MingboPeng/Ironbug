using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingDXMultiSpeedStageData : Ironbug_DuplicableHVACWithParamComponent
    {
        
        public Ironbug_CoilHeatingDXMultiSpeedStageData()
          : base("IB_CoilHeatingDXMultiSpeedStageData", "CoilHtnDX_Stage",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(IB_CoilHeatingDXMultiSpeedStageData_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingDXMultiSpeedStageData", "Stage", "CoilHeatingDXMultiSpeedStageData", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilHeatingDXMultiSpeedStageData();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.DXHeatingStageData;

        public override Guid ComponentGuid => new Guid("1706C5BC-4836-4D50-97FF-C09B560644C5");
    }
}