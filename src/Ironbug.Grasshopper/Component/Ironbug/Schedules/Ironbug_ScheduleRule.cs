using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ScheduleRule : Ironbug_HVACComponent
    {
        
        /// Initializes a new instance of the Ironbug_SizingZone class.
        
        public Ironbug_ScheduleRule()
          : base("Ironbug_ScheduleRule", "ScheduleRule",
              "Description",
              "Ironbug", "07:Curve & Load",
              typeof(HVAC.Schedules.IB_ScheduleRule_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Values", "_value", "One value for all day or 24 values for each hour", GH_ParamAccess.list);
            pManager[pManager.AddTextParameter("Date Range", "dateR_", "Use Ladybug_AnaysisPeriod to create a date range that applies this schedule rule. By default, this rule applies to the entire year.", GH_ParamAccess.list)].Optional =true;

        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ScheduleRule", "schRule", "To Ironbug_Schedule", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var values = new List<double>();
            var dateR = new List<string>();
            DA.GetDataList(0, values);
            DA.GetDataList(1, dateR);

            HVAC.Schedules.IB_ScheduleDay day;
            HVAC.Schedules.IB_ScheduleRule schRule;
            if (values.Count ==1)
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

            if (dateR.Any())
            {
                var d = getDateValues(dateR);
                schRule.SetDateRange(d);
            }

            this.SetObjParamsTo(schRule);
            DA.SetData(0, schRule);

            int [] getDateValues(List<string> date)
            {
                if (date.Count != 2) AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Date range is not valid!");
                var sD = date[0]
                    .Substring(1, date[0].Length - 2)
                    .Split(',')
                    .Select(_=>int.Parse(_.Trim())).ToArray();

                var eD = date[1]
                    .Substring(1, date[1].Length - 2)
                    .Split(',')
                    .Select(_ => int.Parse(_.Trim())).ToArray();

                return new int[] { sD[0], sD[1], eD[0], eD[1] };

            }

        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.Schedule_Rule;

        public override Guid ComponentGuid => new Guid("{E0DEB90F-BA13-49BF-B00A-1EE1AE387746}");
    }
}