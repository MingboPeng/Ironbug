using System;
using System.Linq;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerWarmest : Ironbug_DuplicableHVACComponent
    {

        private static HVAC.IB_SetpointManagerWarmest_FieldSet _fieldSet = HVAC.IB_SetpointManagerWarmest_FieldSet.Value;
        
        public Ironbug_SetpointManagerWarmest()
          : base("IB_SetpointManagerWarmest", "SPM_Warmest",
             "Description",
              "Ironbug", "05:SetpointManager",
              typeof(HVAC.IB_SetpointManagerWarmest_FieldSet))
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("minTemperature", "_minT", _fieldSet.MinimumSetpointTemperature.Description, GH_ParamAccess.item);
            pManager.AddNumberParameter("maxTemperature", "_maxT", _fieldSet.MaximumSetpointTemperature.Description, GH_ParamAccess.item);
            
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerWarmest", "SPM", "TODO:...", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SetpointManagerWarmest();
            double minT = 0;
            double maxT = 0;
            DA.GetData(0, ref minT);
            DA.GetData(1, ref maxT);
            
            obj.SetFieldValue(_fieldSet.MinimumSetpointTemperature, minT);
            obj.SetFieldValue(_fieldSet.MaximumSetpointTemperature, maxT);


            var objs = this.SetObjDupParamsTo(obj);
            if (objs.Count() == 1)
            {
                DA.SetData(0, obj);
            }
            else
            {
                DA.SetDataList(0, objs);
            }
           
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointWarmest;

        public override Guid ComponentGuid => new Guid("9d1e18c2-392e-47ea-8f29-e5407bbd3278");
    }
}