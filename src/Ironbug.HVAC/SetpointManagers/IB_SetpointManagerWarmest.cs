using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerWarmest : IB_SetpointManager
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerWarmest();

        private static SetpointManagerWarmest NewDefaultOpsObj(Model model) => new SetpointManagerWarmest(model);
        public IB_SetpointManagerWarmest() : base(NewDefaultOpsObj(new Model()))
        {
            
        } 

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_SetpointManagerWarmest_FieldSet
        : IB_FieldSet<IB_SetpointManagerWarmest_FieldSet, SetpointManagerWarmest>
    {
        private IB_SetpointManagerWarmest_FieldSet() { }

        public IB_Field MaximumSetpointTemperature { get; }
            = new IB_TopField("MaximumSetpointTemperature", "maxTemp");

        public IB_Field MinimumSetpointTemperature { get; }
            = new IB_TopField("MinimumSetpointTemperature", "minTemp");
    }


}
