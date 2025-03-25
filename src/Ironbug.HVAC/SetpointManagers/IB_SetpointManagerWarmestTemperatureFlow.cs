using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerWarmestTemperatureFlow : IB_SetpointManager
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerWarmestTemperatureFlow();

        private static SetpointManagerWarmestTemperatureFlow NewDefaultOpsObj(Model model) => new SetpointManagerWarmestTemperatureFlow(model);
        public IB_SetpointManagerWarmestTemperatureFlow() : base(NewDefaultOpsObj)
        {
            
        } 

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_SetpointManagerWarmestTemperatureFlow_FieldSet
        : IB_FieldSet<IB_SetpointManagerWarmestTemperatureFlow_FieldSet, SetpointManagerWarmestTemperatureFlow>
    {
        private IB_SetpointManagerWarmestTemperatureFlow_FieldSet() { }

        public IB_Field MaximumSetpointTemperature { get; }
            = new IB_TopField("MaximumSetpointTemperature", "maxT");

        public IB_Field MinimumSetpointTemperature { get; }
            = new IB_TopField("MinimumSetpointTemperature", "minT");
    }


}
