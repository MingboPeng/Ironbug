using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerScheduled : IB_SetpointManager
    {
        private double temperature = 12.7778; //55F

        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerScheduled(this.temperature);

        private static SetpointManagerScheduled NewDefaultOpsObj(Model model, double temp) 
            => new SetpointManagerScheduled(model, new ScheduleRuleset(model, temp));

        public IB_SetpointManagerScheduled(double temperature) : base(NewDefaultOpsObj(new Model(), temperature))
        {
            this.temperature = temperature;

        }

        public override IB_HVACObject Duplicate()
        {
            var newobj = base.Duplicate() as IB_SetpointManagerScheduled;
            newobj.temperature = this.temperature;
            return newobj;
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);


            SetpointManagerScheduled NewDefaultOpsObj(Model m)
            {
                return new SetpointManagerScheduled(m, Schedules.IB_ScheduleRuleset.GetOrNewSchedule(m, this.temperature));
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
