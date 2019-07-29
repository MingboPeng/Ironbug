using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Schedules
{
    public class IB_ScheduleRule : IB_Schedule
    {
        protected override Func<IB_ModelObject> IB_InitSelf
            => () => new IB_ScheduleRule();

        private static ScheduleRule InitMethod(Model model)
            => new ScheduleRule(new ScheduleRuleset(model));


        public IB_ScheduleDay ScheduleDay => this.Children.Get<IB_ScheduleDay>();

        private (int sMonth, int sDay, int eMonth, int eDay) _DateRange = (1, 1, 12, 31);
        public IB_ScheduleRule() : base(InitMethod(new Model()))
        {
        }

        public IB_ScheduleRule(IB_ScheduleDay SchDay) : base(InitMethod(new Model()))
        {
            AddChild(SchDay);
        }

        public void SetDateRange(int[] DateValues)
        {
            this._DateRange = (DateValues[0], DateValues[1], DateValues[2], DateValues[3]);
        }

        public override IB_ModelObject Duplicate()
        {
            var obj = base.Duplicate() as IB_ScheduleRule;
            obj._DateRange = this._DateRange;
            return obj;
        }
        public override ModelObject ToOS(Model model)
        {
            throw new ArgumentException(@"Use 'public ScheduleRule ToOS(ScheduleRuleset Ruleset)' instead!");
        }

        public ScheduleRule ToOS(ScheduleRuleset Ruleset)
        {
            var model = Ruleset.model();
            this.CustomAttributes.TryGetValue(IB_Field_Comment.Instance, out object trackingId);
            var name = $"ScheduleRule - {trackingId.ToString().Substring(12)}";

           
            //There is a bug in ScheduleRule when it is initialized with ScheduleDay, it recreates a new ScheduleDay
            var day = this.ScheduleDay.ToOS(new Model()) as ScheduleDay;
            var obj = new ScheduleRule(Ruleset, day);
            obj.setName(name);
            obj.SetCustomAttributes(this.CustomAttributes);
            obj.setStartDate(new Date(new MonthOfYear(_DateRange.sMonth), (uint)_DateRange.sDay));
            obj.setEndDate(new Date(new MonthOfYear(_DateRange.eMonth), (uint)_DateRange.eDay));
            return obj;
        }
        
    }
    public sealed class IB_ScheduleRule_FieldSet
    : IB_FieldSet<IB_ScheduleRule_FieldSet, ScheduleRule>
    {
        private IB_ScheduleRule_FieldSet() { }

        public IB_Field ApplyMonday { get; }
    = new IB_BasicField("ApplyMonday", "Mon") { };
        public IB_Field ApplyTuesday { get; }
    = new IB_BasicField("ApplyTuesday", "Tue") { };
        public IB_Field ApplyWednesday { get; }
    =   new IB_BasicField("ApplyWednesday", "Wed") { };
        public IB_Field ApplyThursday { get; }
    = new IB_BasicField("ApplyThursday", "Thu") { };
        public IB_Field ApplyFriday { get; }
    = new IB_BasicField("ApplyFriday", "Fri") { };
        public IB_Field ApplySaturday { get; }
    = new IB_BasicField("ApplySaturday", "Sat") { };
        public IB_Field ApplySunday { get; }
    = new IB_BasicField("ApplySunday", "Sun") { };

    }
}