using Ironbug.Core;
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
            this.CustomAttributes.TryGetValue(IB_Field_Name.Instance, out object custName);
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
                if (name.StartsWith("Constant Temperature"))
                {
                    obj = new ScheduleRuleset(model, this.constantNumber);
                }
                else 
                {
                    obj = new ScheduleRuleset(model);
                    name = name.StartsWith("Constant Temperature") ? name : $"Constant value {this.constantNumber}";
                    var defaultDay = obj.defaultDaySchedule();

                    //Check Schedule type
                    var optionalType = model.getScheduleTypeLimitsByName($"Dimensionless max {Math.Round(constantNumber) + 1}");
                    if (optionalType.isNull())
                    {
                        var type = new ScheduleTypeLimits(model);
                        type.setUnitType("Dimensionless");
                        type.setNumericType("Continuous");
                        type.setName($"Dimensionless max {Math.Round(constantNumber) + 1}");
                        type.setLowerLimitValue(0);
                        type.setUpperLimitValue(Math.Round(constantNumber) + 1);
                        obj.setScheduleTypeLimits(type);
                    }
                    else
                    {
                        obj.setScheduleTypeLimits(optionalType.get());
                    }


                    defaultDay.addValue(new Time(0, 24), this.constantNumber);
                }

                //obj = new ScheduleRuleset(model);
               
             
              
                //obj.setName(name);
            }
           
            obj.SetCustomAttributes(this.CustomAttributes);
            return obj;


        }

        public static ScheduleRuleset GetOrNewConstantSchedule(Model m, double value, bool isTemperature = true)
        {
            var name = isTemperature? 
                $"Constant Temperature {Math.Round(value, 1)} C ({Math.Round(value * 9 / 5 + 32, 1)} F) " : 
                $"Constant value {value}";
            var optionalObj = m.getScheduleRulesetByName(name);
            if (optionalObj.is_initialized())
            {
                return optionalObj.get();
            }
            else
            {
                var sch = new IB_ScheduleRuleset(value);
                sch.CustomAttributes.TryAdd(IB_Field_Name.Instance, name);
                return sch.ToOS(m) as ScheduleRuleset;
            }
        }

        //public static ScheduleRuleset GetOrNewSchedule(Model m, double temperature)
        //{
        //    var name = $"Constant value {Math.Round(temperature, 1)} C ({Math.Round(temperature * 9 / 5 + 32, 1)} F) ";
        //    var optionalObj = m.getScheduleRulesetByName(name);
        //    if (optionalObj.is_initialized())
        //    {
        //        return optionalObj.get();
        //    }
        //    else
        //    {
        //        var sch = new ScheduleRuleset(m, temperature);
        //        sch.setName(name);
        //        return sch;
        //    }
        //}
    }
    public sealed class IB_ScheduleRuleset_FieldSet
        : IB_FieldSet<IB_ScheduleRuleset_FieldSet, ScheduleRuleset>
    {
        private IB_ScheduleRuleset_FieldSet() { }
        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name") { };


    }
}