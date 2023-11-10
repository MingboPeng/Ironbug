using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingDXMultiSpeedStageData : Ironbug_DuplicableHVACWithParamComponent
    {
        
        public Ironbug_CoilCoolingDXMultiSpeedStageData()
          : base("IB_CoilCoolingDXMultiSpeedStageData", "CoilClnDX_Stage",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(IB_CoilCoolingDXMultiSpeedStageData_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilCoolingDXMultiSpeedStageData", "Stage", "CoilCoolingDXMultiSpeedStageData", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilCoolingDXMultiSpeedStageData();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.DXCoolingStageData;

        public override Guid ComponentGuid => new Guid("C220EF58-E434-4F19-98B5-AB84D60D1FE2");
    }
}