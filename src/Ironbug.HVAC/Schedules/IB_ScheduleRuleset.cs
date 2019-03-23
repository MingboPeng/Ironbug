using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC.Schedules
{
    public class IB_ScheduleRuleset : IB_Schedule
    {
        private double constantNumber = 0.0;

        protected override Func<IB_ModelObject> IB_InitSelf
            => () => new IB_ScheduleRuleset(this.constantNumber);

        private static ScheduleRuleset InitMethod(Model model)
            => new ScheduleRuleset(model);

        private List<IB_ScheduleRule> Rules { get; set; } = new List<IB_ScheduleRule>();
        public IB_ScheduleRuleset() : base(InitMethod(new Model()))
        {
        }
        public IB_ScheduleRuleset(double value) : base(InitMethod(new Model()))
        {
            this.constantNumber = value;
        }

        public void AddRule(IB_ScheduleRule Rule)
        {
            this.Rules.Add(Rule);
        }
        public override IB_ModelObject Duplicate()
        {
            var obj = base.Duplicate() as IB_ScheduleRuleset;
            foreach (var item in this.Rules)
            {
                obj.AddRule(item.Duplicate() as IB_ScheduleRule);
            }
            
            return obj;
        }

        public override ModelObject ToOS(Model model)
        {
            this.CustomAttributes.TryGetValue(IB_Field_Comment.Instance, out object trackingId);
            var name = $"Schedule - {trackingId}";

            var sch_o = model.getScheduleRulesetByName(name);
            var obj = (ScheduleRuleset)null;
            if (sch_o.is_initialized())
            {
                obj = sch_o.get();
            }
            else if (this.Rules.Count>0)
            {
                obj = new ScheduleRuleset(model, this.constantNumber);
                obj.setName(name);
                var c = this.Rules.Count; 

                var defaultDay = obj.defaultDaySchedule();
             
                for (int i = 0; i < c; i++)
                {
                    var r = this.Rules[i];

                    if (i == c-1)
                    {
                        r.ScheduleDay.CopyValuesToExisting(defaultDay);
                    }
                    else
                    {
                        obj.setScheduleRuleIndex(r.ToOS(obj), (uint)i);
                    }
                

                }
           
               
            }
            else
            {
                name = $"Constant Schedule {this.constantNumber}";
                obj = new ScheduleRuleset(model, this.constantNumber);
                obj.setName(name);
            }

            return obj;
        }
    }
}