using Grasshopper.Kernel;
using System;


namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilPerformanceDXCooling : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_CoilPerformanceDXCooling()
          : base("IB_CoilPerformanceDXCooling", "CoilPerformance", "",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_CoilPerformanceDXCooling_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilPerformance", "CoilP", "Add to CoilCoolingDXTwoStageWithHumidityControlMode", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilPerformanceDXCooling();
            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.CoilPerformance;

        public override Guid ComponentGuid => new Guid("A11C79B3-982D-41AF-9EC6-A93853694B74");
    }
}