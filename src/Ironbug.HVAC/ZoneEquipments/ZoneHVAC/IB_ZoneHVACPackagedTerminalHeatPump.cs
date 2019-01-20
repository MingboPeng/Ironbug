using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACPackagedTerminalHeatPump : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACPackagedTerminalHeatPump(_fan, _heatingCoil, _coolingCoil, _supplementalHeatingCoil);

        private static ZoneHVACPackagedTerminalHeatPump NewDefaultOpsObj(Model m, IB_Fan SupplyFan, IB_Coil HeatingCoil, IB_Coil CoolingCoil, IB_Coil SupplementalHeatingCoil) 
            => new ZoneHVACPackagedTerminalHeatPump(m,m.alwaysOnDiscreteSchedule(), SupplyFan.ToOS(m), HeatingCoil.ToOS(m), CoolingCoil.ToOS(m), SupplementalHeatingCoil.ToOS(m));

        private IB_Coil _coolingCoil => this.Children.Get<IB_Coil>(0);
        private IB_Coil _heatingCoil => this.Children.Get<IB_Coil>(1);
        private IB_Fan _fan => this.Children.Get<IB_Fan>(2);
        private IB_Coil _supplementalHeatingCoil => this.Children.Get<IB_Coil>(3);

        public IB_ZoneHVACPackagedTerminalHeatPump(IB_Fan SupplyFan, IB_Coil HeatingCoil, IB_Coil CoolingCoil, IB_Coil SupplementalHeatingCoil) 
            : base(NewDefaultOpsObj(new Model(),SupplyFan, HeatingCoil, CoolingCoil, SupplementalHeatingCoil))
        {
            this.AddChild(CoolingCoil);
            this.AddChild(HeatingCoil);
            this.AddChild(SupplyFan);
            this.AddChild(SupplementalHeatingCoil);
        }


        protected override ModelObject NewOpsObj(Model model)
        {
            var opsObj =  base.OnNewOpsObj(LocalInitMethod, model).to_ZoneHVACPackagedTerminalHeatPump().get();
            return opsObj;

            ZoneHVACPackagedTerminalHeatPump LocalInitMethod(Model m)
            => new ZoneHVACPackagedTerminalHeatPump(
                m,
                m.alwaysOnDiscreteSchedule(),
                _fan.ToOS(m),
                _heatingCoil.ToOS(m),
                _coolingCoil.ToOS(m),
                _supplementalHeatingCoil.ToOS(m)

                );

        }
    }

    public sealed class IB_ZoneHVACPackagedTerminalHeatPump_DataFieldSet
        : IB_FieldSet<IB_ZoneHVACPackagedTerminalHeatPump_DataFieldSet, ZoneHVACPackagedTerminalHeatPump>
    {
        private IB_ZoneHVACPackagedTerminalHeatPump_DataFieldSet() {}

    }
}
