using Ironbug.HVAC.BaseClass;
using Ironbug.HVAC.Schedules;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerScheduled : IB_SetpointManager
    {

        public double Value { get => Get(12.7778); set=> Set(value, 12.7778); }//55F
        public bool IsTemperature { get => Get(true); set => Set(value, true); }
       


        //private IB_ScheduleRuleset _schedule => this.GetChild<IB_ScheduleRuleset>();
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerScheduled(this.Value);

        private static SetpointManagerScheduled NewDefaultOpsObj(Model model, double temp)
            => new SetpointManagerScheduled(model, new ScheduleRuleset(model, temp));

        private static SetpointManagerScheduled NewDefaultOpsObj(Model model, IB_ScheduleRuleset schedule)
            => new SetpointManagerScheduled(model, schedule.ToOS(model) as ScheduleRuleset);

        public IB_SetpointManagerScheduled(double value) : base(NewDefaultOpsObj(new Model(), value))
        {
            this.Value = value;
        }
        public IB_SetpointManagerScheduled(double value, bool isTemperature) : base(NewDefaultOpsObj(new Model(), value))
        {
            this.Value = value;
            this.IsTemperature = isTemperature;
        }

        public IB_SetpointManagerScheduled() : base(NewDefaultOpsObj(new Model(), -999))
        {
            this.Value = -999;
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);


            SetpointManagerScheduled NewDefaultOpsObj(Model m)
            {
                if (this.Value == -999)
                {
                    //add a placeholder to init SetpointManagerScheduled. 
                    //users can set their own custom attributes
                    return new SetpointManagerScheduled(m, new ScheduleRuleset(m,-999));

                }
                else if (this.IsTemperature)
                {
                    return new SetpointManagerScheduled(m, IB_ScheduleRuleset.GetOrNewConstantSchedule(m, this.Value, this.IsTemperature));
                }
                else
                {
                    this.CustomAttributes.TryGetValue(IB_SetpointManagerScheduled_FieldSet.Value.ControlVariable, out object controlV);
                    var spm = new SetpointManagerScheduled(m, controlV.ToString(), IB_ScheduleRuleset.GetOrNewConstantSchedule(m, this.Value, this.IsTemperature));
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
