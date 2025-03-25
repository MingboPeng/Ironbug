using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACHighTemperatureRadiant : BaseClass.IB_ZoneEquipment
    {
        
        protected override Func<IB_ModelObject> IB_InitSelf 
            => () => new IB_ZoneHVACHighTemperatureRadiant();

        private static ZoneHVACHighTemperatureRadiant NewDefaultOpsObj(Model model) 
            => new ZoneHVACHighTemperatureRadiant(model);

        
        public IB_ZoneHVACHighTemperatureRadiant() 
            : base(NewDefaultOpsObj)
        {
        }
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);

        }
    }

    public sealed class IB_ZoneHVACHighTemperatureRadiant_FieldSet
        : IB_FieldSet<IB_ZoneHVACHighTemperatureRadiant_FieldSet>
    {
        private IB_ZoneHVACHighTemperatureRadiant_FieldSet() { }

    }
}
