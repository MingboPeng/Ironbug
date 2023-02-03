using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerScheduledDualSetpoint : IB_SetpointManager
    {
     
        private double LowT { get => Get(12.7778); set=> Set(value, 12.7778); } //55F
 
        private double HighT { get => Get(21.1); set => Set(value, 21.1); } //70F


        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerScheduledDualSetpoint();

        private static SetpointManagerScheduledDualSetpoint NewDefaultOpsObj(Model model) 
            => new SetpointManagerScheduledDualSetpoint(model);

        [JsonConstructor]
        private IB_SetpointManagerScheduledDualSetpoint(bool forDeserialization) : base(null)
        {
        }

        public IB_SetpointManagerScheduledDualSetpoint(double lowTemperature, double highTemperature) : base(NewDefaultOpsObj(new Model()))
        {
            (this.GhostOSObject as SetpointManagerScheduledDualSetpoint).setControlVariable("Temperature");
            this.LowT = lowTemperature;
            this.HighT = highTemperature;
        }

        public IB_SetpointManagerScheduledDualSetpoint() : base(NewDefaultOpsObj(new Model()))
        {
            this.LowT = -999;
            this.HighT = -999;
        }


        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);

            if (this.LowT !=-999)
            {
                var loSch = Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(model, this.LowT);
                obj.setLowSetpointSchedule(loSch);

                var hiSch = Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(model, this.HighT);
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
