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

        //public IB_ScheduleDay ScheduleTypeLimits => this.Children.Get<IB_ScheduleDay>();

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
            this.CustomAttributes.TryGetValue(IB_ScheduleRuleset_FieldSet.Value.Name, out object custName);
            this.CustomAttributes.TryGetValue(IB_Field_Comment.Instance, out object trackingId);
            var name = custName!= null? custName.ToString():$"Schedule - {trackingId.ToString().Substring(12)}";

            
            var sch_o = model.getScheduleRulesetByName(name);
            var obj = (ScheduleRuleset)null;
            if (sch_o.is_initialized())
            {
                obj = sch_o.get();
            }
            else if (this.Rules.Count>0)
            {
                obj = new ScheduleRuleset(model);
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
                obj.resetScheduleTypeLimits();
           
            }
            else
            {
                name = $"Constant Schedule {this.constantNumber}";
                obj = new ScheduleRuleset(model, this.constantNumber);
                obj.setName(name);
            }
           
            obj.SetCustomAttributes(this.CustomAttributes);
            return obj;
        }

        public static ScheduleRuleset GetOrNewSchedule(Model m, double temperature)
        {
            var name = $"Constant value {Math.Round(temperature, 1)} C ({Math.Round(temperature * 9 / 5 + 32, 1)} F) ";
            var optionalObj = m.getScheduleRulesetByName(name);
            if (optionalObj.is_initialized())
            {
                return optionalObj.get();
            }
            else
            {
                var sch = new ScheduleRuleset(m, temperature);
                sch.setName(name);
                return sch;
            }
        }
    }
    public sealed class IB_ScheduleRuleset_FieldSet
        : IB_FieldSet<IB_ScheduleRuleset_FieldSet, ScheduleRuleset>
    {
        private IB_ScheduleRuleset_FieldSet() { }
        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name") { };

        public IB_Field ScheduleTypeLimits { get; }
            = new IB_BasicField("ScheduleTypeLimits", "ScheduleTypeLimits") { };

    }
}