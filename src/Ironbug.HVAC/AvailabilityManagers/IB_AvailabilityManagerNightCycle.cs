using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.AvailabilityManager
{
    public class IB_AvailabilityManagerNightCycle : IB_AvailabilityManager
    {
        public static IB_AvailabilityManagerNightCycle_FieldSet FieldSet => IB_AvailabilityManagerNightCycle_FieldSet.Value;
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AvailabilityManagerNightCycle();

        private static AvailabilityManagerNightCycle NewDefaultOpsObj(Model model) => new AvailabilityManagerNightCycle(model);
        public IB_AvailabilityManagerNightCycle() : base(NewDefaultOpsObj)
        {
        } 

        public override OpenStudio.AvailabilityManager ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_AvailabilityManagerNightCycle_FieldSet
        : IB_FieldSet<IB_AvailabilityManagerNightCycle_FieldSet, AvailabilityManagerNightCycle>
    {
        private IB_AvailabilityManagerNightCycle_FieldSet() { }

    }


}
