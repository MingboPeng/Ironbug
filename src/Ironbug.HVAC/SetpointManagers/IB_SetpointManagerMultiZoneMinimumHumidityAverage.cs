using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerMultiZoneMinimumHumidityAverage : IB_SetpointManager
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerMultiZoneMinimumHumidityAverage();

        private static SetpointManagerMultiZoneMinimumHumidityAverage NewDefaultOpsObj(Model model)
            => new SetpointManagerMultiZoneMinimumHumidityAverage(model);

        public IB_SetpointManagerMultiZoneMinimumHumidityAverage() : base(NewDefaultOpsObj)
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }
    public sealed class IB_SetpointManagerMultiZoneMinimumHumidityAverage_FieldSet
        : IB_FieldSet<IB_SetpointManagerMultiZoneMinimumHumidityAverage_FieldSet, SetpointManagerMultiZoneMinimumHumidityAverage>
    {
        private IB_SetpointManagerMultiZoneMinimumHumidityAverage_FieldSet() { }

        public IB_Field MaximumSetpointHumidityRatio { get; }
            = new IB_TopField("MaximumSetpointHumidityRatio", "Max");
        public IB_Field MinimumSetpointHumidityRatio { get; }
            = new IB_TopField("MinimumSetpointHumidityRatio", "Min");


    }
}
