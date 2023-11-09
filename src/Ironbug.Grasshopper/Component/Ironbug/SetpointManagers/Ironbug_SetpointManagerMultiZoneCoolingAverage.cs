using System;
using System.Linq;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerMultiZoneCoolingAverage : Ironbug_DuplicableHVACComponent
    {

        private static HVAC.IB_SetpointManagerMultiZoneCoolingAverage_FieldSet _fieldSet = HVAC.IB_SetpointManagerMultiZoneCoolingAverage_FieldSet.Value;
        
        public Ironbug_SetpointManagerMultiZoneCoolingAverage()
          : base("IB_SetpointManagerMultiZoneCoolingAverage", "SPM_ClnAvg",
               "Description",
              "Ironbug", "05:SetpointManager & AvailabilityManager",
              typeof(HVAC.IB_SetpointManagerMultiZoneCoolingAverage_FieldSet))
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("MinimumSetpointTemperature", "_minT", _fieldSet.MinimumSetpointTemperature.Description, GH_ParamAccess.item);
            pManager.AddNumberParameter("MaximumSetpointTemperature", "_maxT", _fieldSet.MaximumSetpointTemperature.Description, GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerMultiZoneCoolingAverage", "SPM", "TODO:...", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SetpointManagerMultiZoneCoolingAverage();
            double min = 0;
            double max = 0;

            if (DA.GetData(0, ref min))
            {
                obj.SetFieldValue(_fieldSet.MinimumSetpointTemperature, min);
            }

            if (DA.GetData(1, ref max))
            {
                obj.SetFieldValue(_fieldSet.MaximumSetpointTemperature, max);
            }

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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointHumidityAvg;

        public override Guid ComponentGuid => new Guid("{D65C29C5-11B4-4F73-B9BA-5B3838E83902}");
    }
}