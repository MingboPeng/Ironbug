using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_FanSystemModel_CV : Ironbug_FanSystemModel
    {
        
        public Ironbug_FanSystemModel_CV() : base()
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

        
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

        public override Guid ComponentGuid => new Guid("0E218A35-BAC3-4A7E-BFB1-DC2F848DD170");


    }

   
}