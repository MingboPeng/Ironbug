using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.AvailabilityManager
{
    public class IB_AvailabilityManagerScheduledOff : IB_AvailabilityManager
    {
        public static IB_AvailabilityManagerScheduledOff_FieldSet FieldSet => IB_AvailabilityManagerScheduledOff_FieldSet.Value;
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AvailabilityManagerScheduledOff();

        private static AvailabilityManagerScheduledOff NewDefaultOpsObj(Model model) => new AvailabilityManagerScheduledOff(model);
        public IB_AvailabilityManagerScheduledOff() : base(NewDefaultOpsObj)
        {
        } 

        public override OpenStudio.AvailabilityManager ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_AvailabilityManagerScheduledOff_FieldSet
        : IB_FieldSet<IB_AvailabilityManagerScheduledOff_FieldSet, AvailabilityManagerScheduledOff>
    {
        private IB_AvailabilityManagerScheduledOff_FieldSet() { }

    }


}
