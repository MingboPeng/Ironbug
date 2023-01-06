using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACPackagedTerminalAirConditioner : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACPackagedTerminalAirConditioner(_fan, _heatingCoil, _coolingCoil);

        private static ZoneHVACPackagedTerminalAirConditioner NewDefaultOpsObj(Model model, IB_Fan SupplyFan, IB_CoilHeatingBasic HeatingCoil, IB_Coil CoolingCoil) 
            => new ZoneHVACPackagedTerminalAirConditioner(model,model.alwaysOnDiscreteSchedule(), SupplyFan.ToOS(model), HeatingCoil.ToOS(model), CoolingCoil.ToOS(model));

        private IB_Coil _coolingCoil => this.GetChild<IB_Coil>(0);
        private IB_CoilHeatingBasic _heatingCoil => this.GetChild<IB_CoilHeatingBasic>(1);
        private IB_Fan _fan => this.GetChild<IB_Fan>(2);

        private IB_ZoneHVACPackagedTerminalAirConditioner() : base(null) { }
        public IB_ZoneHVACPackagedTerminalAirConditioner(IB_Fan SupplyFan, IB_CoilHeatingBasic HeatingCoil, IB_Coil CoolingCoil) 
            : base(NewDefaultOpsObj(new Model(),SupplyFan, HeatingCoil, CoolingCoil))
        {
            this.AddChild(CoolingCoil);
            this.AddChild(HeatingCoil);
            this.AddChild(SupplyFan);
        }

        public override HVACComponent ToOS(Model model)
        {
            var opsObj = base.OnNewOpsObj(LocalInitMethod, model);
            return opsObj;

            ZoneHVACPackagedTerminalAirConditioner LocalInitMethod(Model m)
            => new ZoneHVACPackagedTerminalAirConditioner(
                m,
                m.alwaysOnDiscreteSchedule(),
                _fan.ToOS(m),
                _heatingCoil.ToOS(m),
                _coolingCoil.ToOS(m)
                );
        }
        
    }

    public sealed class IB_ZoneHVACPackagedTerminalAirConditioner_FieldSet
        : IB_FieldSet<IB_ZoneHVACPackagedTerminalAirConditioner_FieldSet, ZoneHVACPackagedTerminalAirConditioner>
    {
        private IB_ZoneHVACPackagedTerminalAirConditioner_FieldSet() {}

    }
}
