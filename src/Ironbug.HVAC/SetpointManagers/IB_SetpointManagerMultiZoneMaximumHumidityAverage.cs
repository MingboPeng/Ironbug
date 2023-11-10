using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerMultiZoneMaximumHumidityAverage : IB_SetpointManager
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerMultiZoneMaximumHumidityAverage();

        private static SetpointManagerMultiZoneMaximumHumidityAverage NewDefaultOpsObj(Model model)
            => new SetpointManagerMultiZoneMaximumHumidityAverage(model);

        public IB_SetpointManagerMultiZoneMaximumHumidityAverage() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }
    public sealed class IB_SetpointManagerMultiZoneMaximumHumidityAverage_FieldSet
        : IB_FieldSet<IB_SetpointManagerMultiZoneMaximumHumidityAverage_FieldSet, SetpointManagerMultiZoneMaximumHumidityAverage>
    {
        private IB_SetpointManagerMultiZoneMaximumHumidityAverage_FieldSet() { }

        public IB_Field MaximumSetpointHumidityRatio { get; }
            = new IB_TopField("MaximumSetpointHumidityRatio", "Max");
        public IB_Field MinimumSetpointHumidityRatio { get; }
            = new IB_TopField("MinimumSetpointHumidityRatio", "Min");


    }
}
