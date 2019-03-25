using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_HeatExchangerAirToAirSensibleAndLatent : IB_HVACObject, IIB_AirLoopObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_HeatExchangerAirToAirSensibleAndLatent();

        private static HeatExchangerAirToAirSensibleAndLatent NewDefaultOpsObj(Model model) => new HeatExchangerAirToAirSensibleAndLatent(model);

        public IB_HeatExchangerAirToAirSensibleAndLatent() : base(NewDefaultOpsObj(new Model()))
        {
        }
         

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_HeatExchangerAirToAirSensibleAndLatent_FieldSet
        : IB_FieldSet<IB_HeatExchangerAirToAirSensibleAndLatent_FieldSet, HeatExchangerAirToAirSensibleAndLatent>
    {
        private IB_HeatExchangerAirToAirSensibleAndLatent_FieldSet() {}
    }
}