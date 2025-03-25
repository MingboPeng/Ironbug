using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerMultiZoneHeatingAverage : IB_SetpointManager
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerMultiZoneHeatingAverage();

        private static SetpointManagerMultiZoneHeatingAverage NewDefaultOpsObj(Model model)
            => new SetpointManagerMultiZoneHeatingAverage(model);

        public IB_SetpointManagerMultiZoneHeatingAverage() : base(NewDefaultOpsObj)
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }
    public sealed class IB_SetpointManagerMultiZoneHeatingAverage_FieldSet
        : IB_FieldSet<IB_SetpointManagerMultiZoneHeatingAverage_FieldSet, SetpointManagerMultiZoneHeatingAverage>
    {
        private IB_SetpointManagerMultiZoneHeatingAverage_FieldSet() { }

        public IB_Field MaximumSetpointTemperature { get; }
            = new IB_TopField("MaximumSetpointTemperature", "MaxT");
        public IB_Field MinimumSetpointTemperature { get; }
            = new IB_TopField("MinimumSetpointTemperature", "MinT");


    }
}
