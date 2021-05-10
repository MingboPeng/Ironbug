using Ironbug.HVAC.BaseClass;
using Ironbug.HVAC.Schedules;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerScheduled : IB_SetpointManager
    {
        private double _value = 12.7778; //55F
        private bool _isTemperature = true;

        //private IB_ScheduleRuleset _schedule => this.GetChild<IB_ScheduleRuleset>();
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerScheduled(this._value);

        private static SetpointManagerScheduled NewDefaultOpsObj(Model model, double temp)
            => new SetpointManagerScheduled(model, new ScheduleRuleset(model, temp));

        private static SetpointManagerScheduled NewDefaultOpsObj(Model model, IB_ScheduleRuleset schedule)
            => new SetpointManagerScheduled(model, schedule.ToOS(model) as ScheduleRuleset);

        public IB_SetpointManagerScheduled(double value) : base(NewDefaultOpsObj(new Model(), value))
        {
            this._value = value;
        }
        public IB_SetpointManagerScheduled(double value, bool isTemperature) : base(NewDefaultOpsObj(new Model(), value))
        {
            this._value = value;
            this._isTemperature = isTemperature;
        }

        public IB_SetpointManagerScheduled() : base(NewDefaultOpsObj(new Model(), -999))
        {
            this._value = -999;
        }

        public override IB_ModelObject Duplicate()
        {
            var newobj = base.Duplicate() as IB_SetpointManagerScheduled;
            newobj._value = this._value;
            newobj._isTemperature = this._isTemperature;
            return newobj;
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);


            SetpointManagerScheduled NewDefaultOpsObj(Model m)
            {
                if (this._value == -999)
                {
                    //add a placeholder to init SetpointManagerScheduled. 
                    //users can set their own custom attributes
                    return new SetpointManagerScheduled(m, new ScheduleRuleset(m,-999));

                }
                else if (this._isTemperature)
                {
                    return new SetpointManagerScheduled(m, IB_ScheduleRuleset.GetOrNewConstantSchedule(m, this._value, this._isTemperature));
                }
                else
                {
                    this.CustomAttributes.TryGetValue(IB_SetpointManagerScheduled_FieldSet.Value.ControlVariable, out object controlV);
                    var spm = new SetpointManagerScheduled(m, controlV.ToString(), IB_ScheduleRuleset.GetOrNewConstantSchedule(m, this._value, this._isTemperature));
                    return spm;
                }
                
            }
        }
    }
    public sealed class IB_SetpointManagerScheduled_FieldSet
      : IB_FieldSet<IB_SetpointManagerScheduled_FieldSet, SetpointManagerScheduled>
    {
        private IB_SetpointManagerScheduled_FieldSet() { }

        public IB_Field ControlVariable { get; }
            = new IB_BasicField("ControlVariable", "ControlVariable");

    }
}
