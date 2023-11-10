using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingGasMultiStageStageData : Ironbug_DuplicableHVACWithParamComponent
    {
        
        public Ironbug_CoilHeatingGasMultiStageStageData()
          : base("IB_CoilHeatingGasMultiStageStageData", "CoilHtnGas_Stage",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(IB_CoilHeatingGasMultiStageStageData_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingGasMultiStageStageData", "Stage", "CoilHeatingGasMultiStageStageData", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilHeatingGasMultiStageStageData();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilHG;

        public override Guid ComponentGuid => new Guid("BCD34B0A-53B5-4500-B21C-8A264BB685DF");
    }
}