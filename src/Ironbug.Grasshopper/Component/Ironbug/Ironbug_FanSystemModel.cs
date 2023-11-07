using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_FanSystemModel : Ironbug_DuplicableHVACWithParamComponent
    {
        
        public Ironbug_FanSystemModel()
          : base("IB_FanSystemModel", "FanSysModel",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_FanSystemModel_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FanSystemModel", "Fan", "Todo..", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_FanSystemModel();


            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Resources.FanSysModel;

        public override Guid ComponentGuid => new Guid("987BB350-7D5D-4AA2-BC16-75B1CDE06DD3");


    }

   
}