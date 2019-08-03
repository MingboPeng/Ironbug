using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_FanOnOff : Ironbug_DuplicatableHVACComponent
    {
        
        public Ironbug_FanOnOff()
          : base("Ironbug_FanOnOff", "FanOnOff",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_FanOnOff_FieldSet))
        {
            
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FanOnOff", "Fan", "Todo..", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_FanOnOff();


            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Resources.FanOnOff;

        public override Guid ComponentGuid => new Guid("028DE04F-F0C3-4DEB-8232-EFFD72BA8AAE");


    }

   
}