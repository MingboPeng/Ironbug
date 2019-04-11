using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerMultiZoneHumidityMinimum : IB_SetpointManager
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerMultiZoneHumidityMinimum();

        private static SetpointManagerMultiZoneHumidityMinimum NewDefaultOpsObj(Model model)
            => new SetpointManagerMultiZoneHumidityMinimum(model);

        public IB_SetpointManagerMultiZoneHumidityMinimum() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }
    public sealed class IB_SetpointManagerMultiZoneHumidityMinimum_FieldSet
        : IB_FieldSet<IB_SetpointManagerMultiZoneHumidityMinimum_FieldSet, SetpointManagerMultiZoneHumidityMinimum>
    {
        private IB_SetpointManagerMultiZoneHumidityMinimum_FieldSet() { }

        public IB_Field MaximumSetpointHumidityRatio { get; }
            = new IB_TopField("MaximumSetpointHumidityRatio", "Max");
        public IB_Field MinimumSetpointHumidityRatio { get; }
            = new IB_TopField("MinimumSetpointHumidityRatio", "Min");


    }
}
