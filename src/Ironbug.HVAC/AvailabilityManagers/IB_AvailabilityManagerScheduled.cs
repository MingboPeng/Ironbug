using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.AvailabilityManager
{
    public class IB_AvailabilityManagerScheduled : IB_AvailabilityManager
    {
        public static IB_AvailabilityManagerScheduled_FieldSet FieldSet => IB_AvailabilityManagerScheduled_FieldSet.Value;
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AvailabilityManagerScheduled();

        private static AvailabilityManagerScheduled NewDefaultOpsObj(Model model) => new AvailabilityManagerScheduled(model);
        public IB_AvailabilityManagerScheduled() : base(NewDefaultOpsObj)
        {
        } 

        public override OpenStudio.AvailabilityManager ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_AvailabilityManagerScheduled_FieldSet
        : IB_FieldSet<IB_AvailabilityManagerScheduled_FieldSet, AvailabilityManagerScheduled>
    {
        private IB_AvailabilityManagerScheduled_FieldSet() { }

    }


}
