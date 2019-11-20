using System;
using Ironbug.HVAC.BaseClass;
using Ironbug.HVAC.Schedules;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACLowTemperatureRadiantElectric : BaseClass.IB_ZoneEquipment
    {
        
        protected override Func<IB_ModelObject> IB_InitSelf 
            => () => new IB_ZoneHVACLowTemperatureRadiantElectric();

        private static ZoneHVACLowTemperatureRadiantElectric NewDefaultOpsObj(Model model) 
            => new ZoneHVACLowTemperatureRadiantElectric(model,model.alwaysOnDiscreteSchedule(), IB_ScheduleRuleset.GetOrNewConstantSchedule(model, 21));

        
        public IB_ZoneHVACLowTemperatureRadiantElectric() 
            : base(NewDefaultOpsObj(new Model()))
        {
        }
        
        public override HVACComponent ToOS(Model model)
        {
            var opsObj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            return opsObj;

        }
    }

    public sealed class IB_ZoneHVACLowTemperatureRadiantElectric_FieldSet
        : IB_FieldSet<IB_ZoneHVACLowTemperatureRadiantElectric_FieldSet>
    {
        private IB_ZoneHVACLowTemperatureRadiantElectric_FieldSet() { }

    }
}
