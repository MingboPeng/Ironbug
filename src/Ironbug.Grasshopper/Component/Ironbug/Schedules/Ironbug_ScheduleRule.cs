﻿using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ScheduleRule : Ironbug_HVACWithParamComponent
    {

        /// Initializes a new instance of the Ironbug_SizingZone class.

        public Ironbug_ScheduleRule()
          : base("IB_ScheduleRule", "ScheduleRule",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Schedules.IB_ScheduleRule_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Values", "_value", "One value for all day or 24 values for each hour", GH_ParamAccess.list);
            pManager[pManager.AddGenericParameter("Date Range", "dateR_", "Use LB_AnaysisPeriod to create a date range that applies this schedule rule. By default, this rule applies to the entire year.", GH_ParamAccess.item)].Optional = true;

        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ScheduleRule", "schRule", "To IB_Schedule", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var values = new List<double>();
            object dateR = null;
            DA.GetDataList(0, values);
            DA.GetData(1, ref dateR);

            HVAC.Schedules.IB_ScheduleDay day;
            HVAC.Schedules.IB_ScheduleRule schRule;
            if (values.Count == 1)
            {
                day = new HVAC.Schedules.IB_ScheduleDay(values[0]);
                schRule = new HVAC.Schedules.IB_ScheduleRule(day);

            }
            else
            {
                if (values.Count != 24) AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Need 24 valves");
                day = new HVAC.Schedules.IB_ScheduleDay(values);
                schRule = new HVAC.Schedules.IB_ScheduleRule(day);

            }

            if (dateR != null)
            {
                var d = Helper.GetDateRange(dateR);
                schRule.SetDateRange(d);
            }

            this.SetObjParamsTo(schRule);
            DA.SetData(0, schRule);

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.Schedule_Rule;

        public override Guid ComponentGuid => new Guid("69B1C1D3-2AB4-43B7-8078-74694B28D884");
    }
}