using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerScheduledDualSetpoint : IB_SetpointManager
    {
        private double lowT = 12.7778; //55F
        private double highT = 21.1; //70F

        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerScheduledDualSetpoint();

        private static SetpointManagerScheduledDualSetpoint NewDefaultOpsObj(Model model) 
            => new SetpointManagerScheduledDualSetpoint(model);

        public IB_SetpointManagerScheduledDualSetpoint() : base(NewDefaultOpsObj(new Model()))
        {
            (this.GhostOSObject as SetpointManagerScheduledDualSetpoint).setControlVariable("Temperature");
        }

        public void SetHighTemperature(double Temperature)
        {
            this.highT = Temperature;
        }

        public void SetLowTemperature(double Temperature)
        {
            this.lowT = Temperature;
        }

        public override IB_HVACObject Duplicate()
        {
            var newobj = base.Duplicate() as IB_SetpointManagerScheduledDualSetpoint;
            newobj.lowT = this.lowT;
            newobj.highT = this.highT;
            return newobj;
        }

        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            var hiSch = GetScheduleRuleset(model, this.highT);
            obj.setHighSetpointSchedule(hiSch);

            var loSch = GetScheduleRuleset(model, this.lowT);
            obj.setLowSetpointSchedule(loSch);

            return obj;

            ScheduleRuleset GetScheduleRuleset(Model m, double temp)
            {
                var name = $"Constant value {temp} C ({Math.Round(temp * 9 / 5 + 32, 1)} F) ";
                var optionalObj = m.getScheduleRulesetByName(name);
                if (optionalObj.is_initialized())
                {
                    return optionalObj.get();
                }
                else
                {
                    var sch = new ScheduleRuleset(m, temp);
                    sch.setName(name);
                    return sch;
                }
            }
        }
    }
}
