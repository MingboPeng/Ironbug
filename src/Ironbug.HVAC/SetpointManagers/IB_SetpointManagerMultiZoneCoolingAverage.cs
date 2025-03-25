using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerMultiZoneCoolingAverage : IB_SetpointManager
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerMultiZoneCoolingAverage();

        private static SetpointManagerMultiZoneCoolingAverage NewDefaultOpsObj(Model model)
            => new SetpointManagerMultiZoneCoolingAverage(model);

        public IB_SetpointManagerMultiZoneCoolingAverage() : base(NewDefaultOpsObj)
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }
    public sealed class IB_SetpointManagerMultiZoneCoolingAverage_FieldSet
        : IB_FieldSet<IB_SetpointManagerMultiZoneCoolingAverage_FieldSet, SetpointManagerMultiZoneCoolingAverage>
    {
        private IB_SetpointManagerMultiZoneCoolingAverage_FieldSet() { }

        public IB_Field MaximumSetpointTemperature { get; }
            = new IB_TopField("MaximumSetpointTemperature", "MaxT");
        public IB_Field MinimumSetpointTemperature { get; }
            = new IB_TopField("MinimumSetpointTemperature", "MinT");


    }
}
