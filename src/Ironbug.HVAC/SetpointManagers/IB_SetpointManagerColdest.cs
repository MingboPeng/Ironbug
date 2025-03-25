using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerColdest : IB_SetpointManager
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerColdest();

        private static SetpointManagerColdest NewDefaultOpsObj(Model model) => new SetpointManagerColdest(model);
        public IB_SetpointManagerColdest() : base(NewDefaultOpsObj)
        {
            
        } 

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_SetpointManagerColdest_FieldSet
        : IB_FieldSet<IB_SetpointManagerColdest_FieldSet, SetpointManagerColdest>
    {
        private IB_SetpointManagerColdest_FieldSet() { }

        public IB_Field MaximumSetpointTemperature { get; }
            = new IB_TopField("MaximumSetpointTemperature", "maxTemp");

        public IB_Field MinimumSetpointTemperature { get; }
            = new IB_TopField("MinimumSetpointTemperature", "minTemp");
    }


}
