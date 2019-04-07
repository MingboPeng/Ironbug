using System;

using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerWarmest : Ironbug_Component
    {

        private static HVAC.IB_SetpointManagerWarmest_FieldSet _fieldSet = HVAC.IB_SetpointManagerWarmest_FieldSet.Value;
        
        public Ironbug_SetpointManagerWarmest()
          : base("Ironbug_SetpointManagerWarmest", "SPM_Warmest",
             _fieldSet.OwnerEpNote,
              "Ironbug", "05:SetpointManager")
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
            
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointWarmest;

        public override Guid ComponentGuid => new Guid("9d1e18c2-392e-47ea-8f29-e5407bbd3278");
    }
}