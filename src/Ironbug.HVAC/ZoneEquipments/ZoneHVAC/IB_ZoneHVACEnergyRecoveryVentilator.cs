using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACEnergyRecoveryVentilator : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACEnergyRecoveryVentilator(_heatingExchanger, _supplyFan, _exhaustFan);

        private static ZoneHVACEnergyRecoveryVentilator NewDefaultOpsObj(Model model, IB_HeatExchangerAirToAirSensibleAndLatent HeExchanger, IB_Fan SupplyFan, IB_Fan ExhaustFan) 
            => new ZoneHVACEnergyRecoveryVentilator(model, HeExchanger.ToOS(model), SupplyFan.ToOS(model), ExhaustFan.ToOS(model));
        
        private IB_HeatExchangerAirToAirSensibleAndLatent _heatingExchanger => this.GetChild<IB_HeatExchangerAirToAirSensibleAndLatent>();
        private IB_Fan _supplyFan => this.GetChild<IB_Fan>(1);
        private IB_Fan _exhaustFan => this.GetChild<IB_Fan>(2);
        private IB_ZoneHVACEnergyRecoveryVentilatorController _controller => this.GetChild<IB_ZoneHVACEnergyRecoveryVentilatorController>(3);
        
        private IB_ZoneHVACEnergyRecoveryVentilator() : base(null) { }
        public IB_ZoneHVACEnergyRecoveryVentilator(IB_HeatExchangerAirToAirSensibleAndLatent HeExchanger, IB_Fan SupplyFan, IB_Fan ExhaustFan) 
            : base((Model m) => NewDefaultOpsObj(m, HeExchanger, SupplyFan, ExhaustFan))
        {
            this.AddChild(HeExchanger);
            this.AddChild(SupplyFan);
            this.AddChild(ExhaustFan);
            this.AddChild(null);
        }

        public void SetController(IB_ZoneHVACEnergyRecoveryVentilatorController controller)
        {
            this.SetChild(3, controller);
        }
        

        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(LocalInitMethod, model);
            if (_controller is not null)
            {
                var controller = _controller.ToOS(model);
                obj.setController(controller);
            }

            return obj;

            ZoneHVACEnergyRecoveryVentilator LocalInitMethod(Model m)
            => new ZoneHVACEnergyRecoveryVentilator(
                m,
                _heatingExchanger.ToOS(m),
                _supplyFan.ToOS(m),
                _exhaustFan.ToOS(m)
                );
        }
    }

    public sealed class IB_ZoneHVACEnergyRecoveryVentilator_FieldSet
        : IB_FieldSet<IB_ZoneHVACEnergyRecoveryVentilator_FieldSet, ZoneHVACEnergyRecoveryVentilator>
    {
        private IB_ZoneHVACEnergyRecoveryVentilator_FieldSet() { }

    }
}
