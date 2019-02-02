using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_FanZoneExhaust : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_FanZoneExhaust();

        private static FanZoneExhaust NewDefaultOpsObj(Model model) 
            => new FanZoneExhaust(model);
        

        public IB_FanZoneExhaust() : base(NewDefaultOpsObj(new Model()))
        {
        }
        

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model); 
            
        }
    }

    public sealed class IB_FanZoneExhaust_DataFieldSet
        : IB_FieldSet<IB_FanZoneExhaust_DataFieldSet, FanZoneExhaust>
    {
        private IB_FanZoneExhaust_DataFieldSet() { }

    }
}
