using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerScheduledDualSetpoint : IB_SetpointManager
    {
        private double _lowT = 12.7778; //55F
        private double _highT = 21.1; //70F

        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerScheduledDualSetpoint();

        private static SetpointManagerScheduledDualSetpoint NewDefaultOpsObj(Model model) 
            => new SetpointManagerScheduledDualSetpoint(model);

        public IB_SetpointManagerScheduledDualSetpoint(double lowTemperature, double highTemperature) : base(NewDefaultOpsObj(new Model()))
        {
            (this.GhostOSObject as SetpointManagerScheduledDualSetpoint).setControlVariable("Temperature");
            this._lowT = lowTemperature;
            this._highT = highTemperature;
        }

        public IB_SetpointManagerScheduledDualSetpoint() : base(NewDefaultOpsObj(new Model()))
        {
            this._lowT = -999;
            this._highT = -999;
        }

        public override IB_ModelObject Duplicate()
        {
            var newobj = base.Duplicate() as IB_SetpointManagerScheduledDualSetpoint;
            newobj._lowT = this._lowT;
            newobj._highT = this._highT;
            return newobj;
        }

        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);

            if (this._lowT !=-999)
            {
                var loSch = Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(model, this._lowT);
                obj.setLowSetpointSchedule(loSch);

                var hiSch = Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(model, this._highT);
                obj.setHighSetpointSchedule(hiSch);
            }


            return obj;

        }
    }
    public sealed class IB_SetpointManagerScheduledDualSetpoint_FieldSet
     : IB_FieldSet<IB_SetpointManagerScheduledDualSetpoint_FieldSet, SetpointManagerScheduledDualSetpoint>
    {
        private IB_SetpointManagerScheduledDualSetpoint_FieldSet() { }
        
    }
}
