﻿using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACPackagedTerminalHeatPump : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () 
            => new IB_ZoneHVACPackagedTerminalHeatPump(_fan, _heatingCoil, _coolingCoil, _supplementalHeatingCoil);

        private static ZoneHVACPackagedTerminalHeatPump NewDefaultOpsObj(Model m, IB_Fan SupplyFan, IB_Coil HeatingCoil, IB_Coil CoolingCoil, IB_Coil SupplementalHeatingCoil) 
            => new ZoneHVACPackagedTerminalHeatPump(m,m.alwaysOnDiscreteSchedule(), SupplyFan.ToOS(m), HeatingCoil.ToOS(m), CoolingCoil.ToOS(m), SupplementalHeatingCoil.ToOS(m));

        private IB_Coil _coolingCoil => this.GetChild<IB_Coil>(0);
        private IB_Coil _heatingCoil => this.GetChild<IB_Coil>(1);
        private IB_Fan _fan => this.GetChild<IB_Fan>(2);
        private IB_Coil _supplementalHeatingCoil => this.GetChild<IB_Coil>(3);
        private IB_ZoneHVACPackagedTerminalHeatPump() : base(null) { }
        public IB_ZoneHVACPackagedTerminalHeatPump(IB_Fan SupplyFan, IB_Coil HeatingCoil, IB_Coil CoolingCoil, IB_Coil SupplementalHeatingCoil) 
            : base((Model m) => NewDefaultOpsObj(m,SupplyFan, HeatingCoil, CoolingCoil, SupplementalHeatingCoil))
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

    public sealed class IB_ZoneHVACPackagedTerminalHeatPump_FieldSet
        : IB_FieldSet<IB_ZoneHVACPackagedTerminalHeatPump_FieldSet, ZoneHVACPackagedTerminalHeatPump>
    {
        private IB_ZoneHVACPackagedTerminalHeatPump_FieldSet() {}

    }
}
