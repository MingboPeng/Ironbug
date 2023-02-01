using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Schedules
{
    public class IB_ScheduleTypeLimits : IB_Schedule
    {
        protected override Func<IB_ModelObject> IB_InitSelf
            => () => new IB_ScheduleTypeLimits();

        private static ScheduleTypeLimits InitMethod(Model model)
            => new ScheduleTypeLimits(model);
        

        public IB_ScheduleTypeLimits() : base(InitMethod(new Model()))
        {
        }
        

        public override ModelObject ToOS(Model model)
        {
            return base.OnNewOpsObj(InitMethod, model);
        }

        public static IB_ScheduleTypeLimits_FieldSet FieldSet => IB_ScheduleTypeLimits_FieldSet.Value;

    }
    public sealed class IB_ScheduleTypeLimits_FieldSet
    : IB_FieldSet<IB_ScheduleTypeLimits_FieldSet, ScheduleTypeLimits>
    {
        private IB_ScheduleTypeLimits_FieldSet() { }
        //public IB_Field Name { get; } = new IB_TopField("Name", "Name") { };
        public IB_Field LowerLimitValue { get; } = new IB_TopField("LowerLimitValue", "LowLimit") { };
        public IB_Field UpperLimitValue { get; } = new IB_TopField("UpperLimitValue", "UpLimit") { };

        public IB_Field UnitType { get; } = new IB_TopField("UnitType", "Type") { };
        public IB_Field NumericType { get; } = new IB_TopField("NumericType", "Numeric") { };


    }
}