using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_FanConstantVolume : Ironbug_DuplicatableHVACWithParamComponent
    {
        public Ironbug_FanConstantVolume()
          : base("Ironbug_FanConstantVolume", "FanConstant",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_FanConstantVolume_FieldSet))
        {
            
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FanConstantVolume", "Fan", "connect to airloop's supply side", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_FanConstantVolume();


            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }


        protected override System.Drawing.Bitmap Icon => Resources.FanC;

        public override Guid ComponentGuid => new Guid("f517230e-27e9-4fd0-bfbc-31f0596d35c4");


    }

   
}