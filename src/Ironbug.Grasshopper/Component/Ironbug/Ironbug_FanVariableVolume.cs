using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_FanVariableVolume : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_FanVariableVolume()
          : base("IB_FanVariableVolume", "FanVariable",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_FanVariableVolume_FieldSet))
        {
        }
        
        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary | GH_Exposure.obscure;
        
        
        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FanVariableVolume", "Fan", "connect to airloop's supply side", GH_ParamAccess.item);
        }

        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_FanVariableVolume();


            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Resources.FanV;

        public override Guid ComponentGuid => new Guid("eebe83e8-f84d-492a-8394-0b81ab2002e0");
    }
}