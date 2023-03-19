﻿using System;
using System.Linq;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component.Ironbug
{
    public class Ironbug_SetpointManagerMultiZoneHumidityMinimum : Ironbug_DuplicableHVACComponent
    {

        private static HVAC.IB_SetpointManagerMultiZoneHumidityMinimum_FieldSet _fieldSet = HVAC.IB_SetpointManagerMultiZoneHumidityMinimum_FieldSet.Value;
        
        public Ironbug_SetpointManagerMultiZoneHumidityMinimum()
          : base("IB_SetpointManagerMultiZoneHumidityMinimum", "SPM_HumidityMin",
                "Description",
                "Ironbug", "05:SetpointManager & AvailabilityManager",
                typeof(HVAC.IB_SetpointManagerMultiZoneHumidityMinimum_FieldSet))
        {
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("MinimumSetpointHumidityRatio", "_min", _fieldSet.MinimumSetpointHumidityRatio.Description, GH_ParamAccess.item);
            pManager.AddNumberParameter("MaximumSetpointHumidityRatio", "_max", _fieldSet.MaximumSetpointHumidityRatio.Description, GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager[1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SetpointManagerMultiZoneHumidityMinimum", "SPM", "TODO:...", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_SetpointManagerMultiZoneHumidityMinimum();
            double min = 0;
            double max = 0;
            if (DA.GetData(0, ref min))
            {
                obj.SetFieldValue(_fieldSet.MinimumSetpointHumidityRatio, min);
            }

            if (DA.GetData(1, ref max))
            {
                obj.SetFieldValue(_fieldSet.MaximumSetpointHumidityRatio, max);
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

        protected override System.Drawing.Bitmap Icon => Properties.Resources.SetPointHumidityMin;

        public override Guid ComponentGuid => new Guid("{444370CF-B58B-4270-ADC3-EF7CA26C0424}");
    }
}