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

        private Schedules.IB_ScheduleRuleset _schedule => this.Children.Get<Schedules.IB_ScheduleRuleset>();
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerScheduled(this._value);

        private static SetpointManagerScheduled NewDefaultOpsObj(Model model, double temp) 
            => new SetpointManagerScheduled(model, new ScheduleRuleset(model, temp));

        private static SetpointManagerScheduled NewDefaultOpsObj(Model model, IB_ScheduleRuleset schedule)
            => new SetpointManagerScheduled(model, schedule.ToOS(model) as ScheduleRuleset);

        //TODO: this need to be merged to public IB_SetpointManagerScheduled(IB_ScheduleRuleset schedule) 
        public IB_SetpointManagerScheduled(double value) : base(NewDefaultOpsObj(new Model(), value))
        {
            this._value = value;
        }
        //TODO: this need to be merged to public IB_SetpointManagerScheduled(IB_ScheduleRuleset schedule) 
        public IB_SetpointManagerScheduled(double value, bool isTemperature) : base(NewDefaultOpsObj(new Model(), value))
        {
            this._value = value;
            this._isTemperature = isTemperature;
        }

        public IB_SetpointManagerScheduled(IB_ScheduleRuleset schedule) : base(NewDefaultOpsObj(new Model(), schedule))
        {
            this.SetChild(schedule);
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
                if (this._isTemperature)
                {
                    return new SetpointManagerScheduled(m, Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, this._value, this._isTemperature));
                }
                else
                {
                    this.CustomAttributes.TryGetValue(IB_SetpointManagerScheduled_FieldSet.Value.ControlVariable, out object controlV);
                    var spm = new SetpointManagerScheduled(m, controlV.ToString(), Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, this._value, this._isTemperature));
                    //spm.setControlVariableAndSchedule(controlV.ToString(), Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, this._value, this._isTemperature));
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
