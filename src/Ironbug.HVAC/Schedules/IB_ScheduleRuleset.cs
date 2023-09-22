using Ironbug.Core;
using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC.Schedules
{
    public class IB_ScheduleRuleset : IB_Schedule
    {
        private double constantNumber => this.Get<double>(0.0);

        protected override Func<IB_ModelObject> IB_InitSelf
            => () => new IB_ScheduleRuleset(this.constantNumber);

        private static ScheduleRuleset InitMethod(Model model)
            => new ScheduleRuleset(model);

        public List<IB_ScheduleRule> Rules
        {
            get => this.TryGetList<IB_ScheduleRule>();
            set => this.Set(value);
        }

        public IB_ScheduleTypeLimits ScheduleTypeLimits
        {
            get => this.Get<IB_ScheduleTypeLimits>();
            set => this.Set(value);
        }

        public IB_ScheduleRuleset() : base(InitMethod(new Model()))
        {
        }
        public IB_ScheduleRuleset(double value) : base(InitMethod(new Model()))
        {
            this.Set(nameof(constantNumber), value);
        }

        public override ModelObject ToOS(Model model)
        {
            this.CustomAttributes.TryGetValue(IB_Field_Name.Instance, out object custName);
            this.CustomAttributes.TryGetValue(IB_Field_Comment.Instance, out object trackingId);
            var name = custName != null ? custName.ToString() : $"Schedule - {trackingId.ToString().Substring(12)}";
            var obj = this.GetIfInModel<ScheduleRuleset>(model, this.GetTrackingID());
            
            if (obj != null)
            {
                //do nothing
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
                    //name = name.StartsWith("Constant Temperature") ? name : $"Constant value {this.constantNumber}";
                    var defaultDay = obj.defaultDaySchedule();
                    defaultDay.addValue(new Time(0, 24), this.constantNumber);


                    //Check Schedule type
                    if (this.ScheduleTypeLimits == null)
                    {
                        // create a new default dimensionless ScheduleTypeLimits
                        var optionalType = model.getScheduleTypeLimitsByName($"Dimensionless max {Math.Round(constantNumber) + 1}");
                        if (optionalType.isNull()) // create a new one
                        {

                            var type = new ScheduleTypeLimits(model);
                            type.setUnitType("Dimensionless");
                            type.setNumericType("Continuous");
                            type.setName($"Dimensionless max {Math.Round(constantNumber) + 1}");
                            type.setLowerLimitValue(0);
                            type.setUpperLimitValue(Math.Round(constantNumber) + 1);
                            obj.setScheduleTypeLimits(type);
                        }
                        else // use the previously created one
                        {
                            obj.setScheduleTypeLimits(optionalType.get());
                        }

                    }
                    else // reset the type limits so that it can apply a new one later
                    {
                        obj.resetScheduleTypeLimits();
                    }
                    
                }

            }

            if (this.ScheduleTypeLimits != null)
            {
                obj.setScheduleTypeLimits(this.ScheduleTypeLimits.ToOS(model) as ScheduleTypeLimits);
            }
          
           
            obj.SetCustomAttributes(this.CustomAttributes);

            // schedule rule set can be shared within model
            OpsIDMapper.TryAdd(this.GetTrackingID(), obj.handle());
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
        private IB_ScheduleRuleset_FieldSet() {}
        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name") { };
      

    }
}