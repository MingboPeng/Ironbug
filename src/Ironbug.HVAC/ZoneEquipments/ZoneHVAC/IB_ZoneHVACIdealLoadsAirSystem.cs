using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACIdealLoadsAirSystem : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACIdealLoadsAirSystem();

        private static ZoneHVACIdealLoadsAirSystem NewDefaultOpsObj(Model model) 
            => new ZoneHVACIdealLoadsAirSystem(model);
        

        public IB_ZoneHVACIdealLoadsAirSystem() : base(NewDefaultOpsObj)
        {
        }
        

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model); 
            
        }
    }

    public sealed class IB_ZoneHVACIdealLoadsAirSystem_FieldSet
        : IB_FieldSet<IB_ZoneHVACIdealLoadsAirSystem_FieldSet, ZoneHVACIdealLoadsAirSystem>
    {
        private IB_ZoneHVACIdealLoadsAirSystem_FieldSet() { }

    }
}
