using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACWaterToAirHeatPump : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () 
            => new IB_ZoneHVACWaterToAirHeatPump(_fan, _heatingCoil, _coolingCoil, _supplementalHeatingCoil);

        private static ZoneHVACWaterToAirHeatPump NewDefaultOpsObj(Model m, IB_Fan SupplyFan, IB_Coil HeatingCoil, IB_Coil CoolingCoil, IB_Coil SupplementalHeatingCoil)
            => new ZoneHVACWaterToAirHeatPump(m, m.alwaysOnDiscreteSchedule(), SupplyFan.ToOS(m), HeatingCoil.ToOS(m), CoolingCoil.ToOS(m), SupplementalHeatingCoil.ToOS(m));

        
        private IB_Coil _coolingCoil => this.Children.Get<IB_Coil>(0);
        private IB_Coil _heatingCoil => this.Children.Get<IB_Coil>(1);
        private IB_Fan _fan => this.Children.Get<IB_Fan>(2);
        private IB_Coil _supplementalHeatingCoil => this.Children.Get<IB_Coil>(3);

        public IB_ZoneHVACWaterToAirHeatPump(IB_Fan SupplyFan, IB_Coil HeatingCoil, IB_Coil CoolingCoil, IB_Coil SupplementalHeatingCoil) 
            : base(NewDefaultOpsObj(new Model(), SupplyFan, HeatingCoil, CoolingCoil, SupplementalHeatingCoil))
        {

            this.AddChild(CoolingCoil);
            this.AddChild(HeatingCoil);
            this.AddChild(SupplyFan);
            this.AddChild(SupplementalHeatingCoil);
        }
        

        public override HVACComponent ToOS(Model model)
        {
            var opsObj = base.OnNewOpsObj(LocalInitMethod, model);
            return opsObj;

            ZoneHVACWaterToAirHeatPump LocalInitMethod(Model m)
            => new ZoneHVACWaterToAirHeatPump(
                m,
                m.alwaysOnDiscreteSchedule(),
                _fan.ToOS(m),
                _heatingCoil.ToOS(m),
                _coolingCoil.ToOS(m),
                _supplementalHeatingCoil.ToOS(m)

                );

        }
    }

    public sealed class IB_ZoneHVACWaterToAirHeatPump_FieldSet
        : IB_FieldSet<IB_ZoneHVACWaterToAirHeatPump_FieldSet, ZoneHVACWaterToAirHeatPump>
    {
        private IB_ZoneHVACWaterToAirHeatPump_FieldSet() { }

    }
}
