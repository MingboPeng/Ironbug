using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Schedules
{
    public class IB_ScheduleRuleset : IB_Schedule
    {
        private double constantNumber = 0.0;

        protected override Func<IB_ModelObject> IB_InitSelf
            => () => new IB_ScheduleRuleset(this.constantNumber);

        private static ScheduleRuleset InitMethod(Model model)
            => new ScheduleRuleset(model);

        public IB_ScheduleRuleset(double value) : base(InitMethod(new Model()))
        {
            this.constantNumber = value;
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            //return base.OnInitOpsObj(InitMethod, model).to_ScheduleRuleset().get();
            var name = $"Constant Schedule {this.constantNumber}";
            var sch_o = model.getScheduleRulesetByName(name);
            var obj = (ScheduleRuleset)null;
            if (sch_o.is_initialized())
            {
                obj = sch_o.get();
            }
            else
            {
                obj = new ScheduleRuleset(model);
                obj.setName(name);
            }

            return obj;
        }
    }
}