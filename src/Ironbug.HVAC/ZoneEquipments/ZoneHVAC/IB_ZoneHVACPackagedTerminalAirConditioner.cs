using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACPackagedTerminalAirConditioner : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACPackagedTerminalAirConditioner(_fan, _heatingCoil, _coolingCoil);

        private static ZoneHVACPackagedTerminalAirConditioner NewDefaultOpsObj(Model model, IB_Fan SupplyFan, IB_Coil HeatingCoil, IB_Coil CoolingCoil) 
            => new ZoneHVACPackagedTerminalAirConditioner(model,model.alwaysOnDiscreteSchedule(), SupplyFan.ToOS(model), HeatingCoil.ToOS(model), CoolingCoil.ToOS(model));

        private IB_Coil _coolingCoil => this.Children.Get<IB_Coil>(0);
        private IB_Coil _heatingCoil => this.Children.Get<IB_Coil>(1);
        private IB_Fan _fan => this.Children.Get<IB_Fan>(2);

        public IB_ZoneHVACPackagedTerminalAirConditioner(IB_Fan SupplyFan, IB_Coil HeatingCoil, IB_Coil CoolingCoil) 
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
            throw new NotImplementedException();
        }
        
    }

    public sealed class IB_ZoneHVACPackagedTerminalAirConditioner_DataFieldSet
        : IB_FieldSet<IB_ZoneHVACPackagedTerminalAirConditioner_DataFieldSet, ZoneHVACPackagedTerminalAirConditioner>
    {
        private IB_ZoneHVACPackagedTerminalAirConditioner_DataFieldSet() {}

    }
}
