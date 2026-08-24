using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACEnergyRecoveryVentilatorController : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACEnergyRecoveryVentilatorController();

        private static ZoneHVACEnergyRecoveryVentilatorController NewDefaultOpsObj(Model model) => new ZoneHVACEnergyRecoveryVentilatorController(model);

        public IB_ZoneHVACEnergyRecoveryVentilatorController() : base(NewDefaultOpsObj)
        {
        }

        public ZoneHVACEnergyRecoveryVentilatorController ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_ZoneHVACEnergyRecoveryVentilatorController_FieldSet
        : IB_FieldSet<IB_ZoneHVACEnergyRecoveryVentilatorController_FieldSet, ZoneHVACEnergyRecoveryVentilatorController>
    {
        private IB_ZoneHVACEnergyRecoveryVentilatorController_FieldSet() {}
    }
}
