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
        
        private IB_HeatExchangerAirToAirSensibleAndLatent _heatingExchanger => this.Children.Get<IB_HeatExchangerAirToAirSensibleAndLatent>();
        private IB_Fan _supplyFan => this.Children.Get<IB_Fan>(1);
        private IB_Fan _exhaustFan => this.Children.Get<IB_Fan>(2);

        public IB_ZoneHVACEnergyRecoveryVentilator(IB_HeatExchangerAirToAirSensibleAndLatent HeExchanger, IB_Fan SupplyFan, IB_Fan ExhaustFan) 
            : base(NewDefaultOpsObj(new Model(), HeExchanger, SupplyFan, ExhaustFan))
        {
            this.AddChild(HeExchanger);
            this.AddChild(SupplyFan);
            this.AddChild(ExhaustFan);
        }
        

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(LocalInitMethod, model);

            ZoneHVACEnergyRecoveryVentilator LocalInitMethod(Model m)
            => new ZoneHVACEnergyRecoveryVentilator(
                m,
                _heatingExchanger.ToOS(m),
                _supplyFan.ToOS(m),
                _exhaustFan.ToOS(m)
                );
        }
    }

    public sealed class IB_ZoneHVACEnergyRecoveryVentilator_DataFieldSet
        : IB_FieldSet<IB_ZoneHVACEnergyRecoveryVentilator_DataFieldSet, ZoneHVACEnergyRecoveryVentilator>
    {
        private IB_ZoneHVACEnergyRecoveryVentilator_DataFieldSet() { }

    }
}
