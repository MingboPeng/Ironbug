using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC.Schedules
{
    public class IB_ScheduleDay : IB_Schedule
    {
  
        private double constantNumber { get => this.Get(0.0); set => this.Set(value, 0.0); }
        private List<double> values { get => this.TryGetList<double>(); set => this.Set(value); }

        protected override Func<IB_ModelObject> IB_InitSelf
            => () => new IB_ScheduleDay(constantNumber);

        private static ScheduleDay InitMethod(Model model)
            => new ScheduleDay(model);

        private IB_ScheduleDay() : base(null) { }
        public IB_ScheduleDay(double constantValue) : base(InitMethod(new Model()))
        {
            this.constantNumber = constantValue;
        }
        public IB_ScheduleDay(List<double> valuesFor24Hrs) : base(InitMethod(new Model()))
        {
            if (valuesFor24Hrs.Count != 24) throw new ArgumentException("24 values are needed");
            values = valuesFor24Hrs;
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
            if (values.Count==0)
            {

                ScheduleDay.addValue(new Time(0, 24), this.constantNumber);
            }
            else
            {
                //int hr = 1;
                var previousValue = values[0];
                var hrCount = values.Count;
                for (int i = 1; i < hrCount; i++)
                {
                    //hr = i+1;
                    var value = values[i];
                    if (value != previousValue)
                    {

                        ScheduleDay.addValue(new Time(0, i), previousValue);
                        previousValue = value;

                    }
                    if (i == hrCount - 1)
                    {
                        ScheduleDay.addValue(new Time(0, i + 1), value);
                    }
                }
            }
            
         
        }
    }
}