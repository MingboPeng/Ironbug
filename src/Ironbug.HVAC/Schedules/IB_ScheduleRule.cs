﻿using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC.Schedules
{
    public class IB_ScheduleRule : IB_Schedule
    {
        protected override Func<IB_ModelObject> IB_InitSelf
            => () => new IB_ScheduleRule();

        private static ScheduleRule InitMethod(Model model)
            => new ScheduleRule(new ScheduleRuleset(model));


        public IB_ScheduleDay ScheduleDay => this.GetChild<IB_ScheduleDay>();
        private List<int> _dateRange 
        {
            get => this.GetList(initDefault: () => new List<int> { 1, 1, 12, 31 });
            set => this.Set(value); 
        }
        //private (int sMonth, int sDay, int eMonth, int eDay) dateRange => (_dateRange[0], _dateRange[1], _dateRange[2], _dateRange[3]);
        private int dateRangeMonthStart => _dateRange[0];
        private int dateRangeDayStart => _dateRange[1];
        private int dateRangeMonthEnd => _dateRange[2];
        private int dateRangeDayEnd => _dateRange[3];
        public IB_ScheduleRule() : base(InitMethod)
        {
        }

        public IB_ScheduleRule(IB_ScheduleDay SchDay) : base(InitMethod)
        {
            AddChild(SchDay);
        }

        public void SetDateRange(int[] DateValues)
        {
            _dateRange = DateValues.ToList();
        }

        public override ModelObject ToOS(Model model)
        {
            throw new ArgumentException(@"Use 'public ScheduleRule ToOS(ScheduleRuleset Ruleset)' instead!");
        }

        public ScheduleRule ToOS(Model model, ScheduleRuleset ruleset)
        {
            this.CustomAttributes.TryGetValue(IB_Field_Comment.Instance, out object trackingId);
            var name = $"ScheduleRule - {trackingId.ToString().Substring(12)}";

           
            //There is a bug in ScheduleRule when it is initialized with ScheduleDay, it recreates a new ScheduleDay
            var day = this.ScheduleDay.ToOS(new Model()) as ScheduleDay;
            var obj = new ScheduleRule(ruleset, day);
            obj.setName(name);
            obj.SetCustomAttributes(model, this.CustomAttributes);
            obj.setStartDate(new Date(new MonthOfYear(dateRangeMonthStart), (uint)dateRangeDayStart));
            obj.setEndDate(new Date(new MonthOfYear(dateRangeMonthEnd), (uint)dateRangeDayEnd));
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