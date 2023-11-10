using System;
using System.Linq;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerWarmestTemperatureFlow : Ironbug_DuplicableHVACComponent
    {

        private static HVAC.IB_SetpointManagerWarmestTemperatureFlow_FieldSet _fieldSet = HVAC.IB_SetpointManagerWarmestTemperatureFlow_FieldSet.Value;
        
        public Ironbug_SetpointManagerWarmestTemperatureFlow()
          : base("IB_SetpointManagerWarmestTemperatureFlow", "SPM_WarmestTempFlow",
             "Description",
              "Ironbug", "05:SetpointManager & AvailabilityManager",
              typeof(HVAC.IB_SetpointManagerWarmestTemperatureFlow_FieldSet))
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
            pManager.AddGenericParameter("SetpointManagerWarmestTemperatureFlow", "SPM", "Setpoint Manager Warmest Temperature Flow", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SetpointManagerWarmestTemperatureFlow();
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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SPM_WarmestTempFlow;

        public override Guid ComponentGuid => new Guid("{825B3236-3DF1-4004-900B-0B199E0A72FD}");
    }
}