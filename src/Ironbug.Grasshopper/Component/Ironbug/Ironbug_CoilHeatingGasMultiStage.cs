using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilHeatingGasMultiStage : Ironbug_HVACWithParamComponent
    {
        public Ironbug_CoilHeatingGasMultiStage()
          : base("IB_CoilHeatingGasMultiStage", "CoilHtnGasMlt",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilHeatingGasMultiStage_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;
        
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingGasMultiStageStageData", "Stages", "A list of IB_CoilHeatingGasMultiStageStageData", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingGasMultiStage", "Coil", "CoilHeatingGasMultiStage", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var stages = new List<IB_CoilHeatingGasMultiStageStageData>();
            if (!DA.GetDataList(0, stages))
                return;

            var obj = new HVAC.IB_CoilHeatingGasMultiStage();
            obj.SetStages(stages);

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilHDXM;

        public override Guid ComponentGuid => new Guid("2882ACCE-DDBE-48AB-BC62-6179E9406CEB");
    }
}