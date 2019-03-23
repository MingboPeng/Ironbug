using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC.Schedules
{
    public class IB_ScheduleDay : IB_Schedule
    {
        protected override Func<IB_ModelObject> IB_InitSelf
            => () => new IB_ScheduleDay(this.values);

        private static ScheduleDay InitMethod(Model model)
            => new ScheduleDay(model);

        private List<double> values { get; set; } = new List<double>();

        //public IB_ScheduleDay() : base(InitMethod(new Model()))
        //{
        //}
        public IB_ScheduleDay(List<double> ValuesFor24Hrs) : base(InitMethod(new Model()))
        {
            if (ValuesFor24Hrs.Count != 24) throw new ArgumentException("24 values are needed");
            this.values = ValuesFor24Hrs;
        }
        public override IB_ModelObject Duplicate()
        {
            var obj = base.Duplicate() as IB_ScheduleDay;
            obj.values = this.values;
            return obj;
        }
        public override ModelObject ToOS(Model model)
        {
            this.CustomAttributes.TryGetValue(IB_Field_Comment.Instance, out object trackingId);
            var name = $"ScheduleDay - {trackingId.ToString().Substring(12)}";
            var sch_o = model.getScheduleDayByName(name);
            var obj = (ScheduleDay)null;
            if (sch_o.is_initialized())
            {
                obj = sch_o.get();
            }
            else
            {
                obj = new ScheduleDay(model);
                obj.setName(name);
                CopyValuesToExisting(obj);
                
            }

            return obj;
        }

        public void CopyValuesToExisting(ScheduleDay ScheduleDay)
        {
            ScheduleDay.clearValues();
            var values = this.values;
            int hr = 1;
            var previousValue = 0.0;
            for (int i = 0; i < values.Count; i++)
            {
                hr = i+1;
                var value = values[i];
                if (i ==0 )
                {
                    previousValue = value;
                    ScheduleDay.addValue(new Time(0, i), value);
                }
                else if (value != previousValue)
                {
                    
                    ScheduleDay.addValue(new Time(0, hr-1), previousValue);
                    previousValue = value;
                }

            }
         
        }
    }
}