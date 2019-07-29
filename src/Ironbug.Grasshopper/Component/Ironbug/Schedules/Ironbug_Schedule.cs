using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_Schedule : Ironbug_Component
    {
        public Ironbug_Schedule()
          : base("Ironbug_Schedule", "Schedule",
              HVAC.Schedules.IB_ScheduleRuleset_FieldSet.Value.OwnerEpNote,
              "Ironbug", "07:Curve & Load")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("ScheduleName", "name_", "Name for this schedule", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("ValuesOrRules", "_rules", "One value for all day or 24 value for each hour, or use a list of scheduleRules from Ironbug_ScheduleRules.\nThe last ScheduleRule will be set to the default (base) schedule.", GH_ParamAccess.list);
            pManager.AddGenericParameter("ScheduleType", "type_", "Use Ironbug_ScheduleType", GH_ParamAccess.item);
            pManager[2].Optional = true;
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Schedule", "sch", "TODO..", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
           
            var attributes = HVAC.Schedules.IB_ScheduleRuleset_FieldSet.Value;
            var objAttris = new Dictionary<IB_Field, object>() { };
            var name = string.Empty;
            if (DA.GetData(0, ref name))
            {
                objAttris.Add(attributes.Name, name);
            }

            HVAC.Schedules.IB_ScheduleTypeLimits scheduleType = null;
            if (DA.GetData(2, ref scheduleType))
            {
                objAttris.Add(attributes.ScheduleTypeLimits, scheduleType);
            }


            var ghObjs = new List<GH_ObjectWrapper>();
            DA.GetDataList(1, ghObjs);
            var sch = new HVAC.Schedules.IB_ScheduleRuleset();

            if (ghObjs[0].Value is GH_Number)
            {
                var values = ghObjs.Select(_ => Double.Parse(_.Value.ToString())).ToList();
                if (values.Count == 1)
                {
                    sch = new HVAC.Schedules.IB_ScheduleRuleset(values[0]);
                    sch.SetFieldValues(objAttris);
                    DA.SetData(0, sch);
                    return;
                }

                if (values.Count != 24) AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Need 24 valves");
                var day = new HVAC.Schedules.IB_ScheduleDay(values);
                var schRule = new HVAC.Schedules.IB_ScheduleRule(day);
                
                sch.AddRule(schRule);

            }
            else if (ghObjs[0].Value is HVAC.Schedules.IB_ScheduleRule)
            {
                var values = ghObjs.Select(_ => _.Value as HVAC.Schedules.IB_ScheduleRule);

                foreach (var item in values)
                {
                    sch.AddRule(item);
                }
                
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Unknown input value. Use 24 numbers for a typical day schedule or a list of ScheduleRules");
            }

            sch.SetFieldValues(objAttris);
            DA.SetData(0, sch);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.Schedule;

        public override Guid ComponentGuid => new Guid("AB6CC8DE-828F-4413-B52A-B49BE6CEE48C");
    }
}