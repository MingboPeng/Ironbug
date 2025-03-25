using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerMultiZoneHumidityMaximum : IB_SetpointManager
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerMultiZoneHumidityMaximum();

        private static SetpointManagerMultiZoneHumidityMaximum NewDefaultOpsObj(Model model)
            => new SetpointManagerMultiZoneHumidityMaximum(model);

        public IB_SetpointManagerMultiZoneHumidityMaximum() : base(NewDefaultOpsObj)
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }
    public sealed class IB_SetpointManagerMultiZoneHumidityMaximum_FieldSet
        : IB_FieldSet<IB_SetpointManagerMultiZoneHumidityMaximum_FieldSet, SetpointManagerMultiZoneHumidityMaximum>
    {
        private IB_SetpointManagerMultiZoneHumidityMaximum_FieldSet() { }

        public IB_Field MaximumSetpointHumidityRatio { get; }
            = new IB_TopField("MaximumSetpointHumidityRatio", "Max");
        public IB_Field MinimumSetpointHumidityRatio { get; }
            = new IB_TopField("MinimumSetpointHumidityRatio", "Min");


    }
}
