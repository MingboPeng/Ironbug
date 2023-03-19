using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.AvailabilityManager
{
    public class IB_AvailabilityManagerScheduledOn : IB_AvailabilityManager
    {
        public static IB_AvailabilityManagerScheduledOn_FieldSet FieldSet => IB_AvailabilityManagerScheduledOn_FieldSet.Value;
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AvailabilityManagerScheduledOn();

        private static AvailabilityManagerScheduledOn NewDefaultOpsObj(Model model) => new AvailabilityManagerScheduledOn(model);
        public IB_AvailabilityManagerScheduledOn() : base(NewDefaultOpsObj(new Model()))
        {
        } 

        public override OpenStudio.AvailabilityManager ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_AvailabilityManagerScheduledOn_FieldSet
        : IB_FieldSet<IB_AvailabilityManagerScheduledOn_FieldSet, AvailabilityManagerScheduledOn>
    {
        private IB_AvailabilityManagerScheduledOn_FieldSet() { }

    }


}
