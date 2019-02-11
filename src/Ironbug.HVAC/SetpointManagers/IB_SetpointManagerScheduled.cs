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
                var sch = new ScheduleRuleset(m, this.temperature);
                sch.setName("Constant value at " + this.temperature);
                return new SetpointManagerScheduled(m, sch);
            }
        }
    }
}
