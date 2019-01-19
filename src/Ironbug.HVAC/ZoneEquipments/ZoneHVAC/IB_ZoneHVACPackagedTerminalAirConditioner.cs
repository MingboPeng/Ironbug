using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACPackagedTerminalAirConditioner : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACPackagedTerminalAirConditioner(Fan, HeatingCoil, CoolingCoil);

        private static ZoneHVACPackagedTerminalAirConditioner NewDefaultOpsObj(Model model, IB_Fan SupplyFan, IB_CoilHeatingBasic HeatingCoil, IB_CoilCoolingBasic CoolingCoil) 
            => new ZoneHVACPackagedTerminalAirConditioner(model,model.alwaysOnDiscreteSchedule(), SupplyFan.ToOS(model), HeatingCoil.ToOS(model), CoolingCoil.ToOS(model));

        private IB_CoilCoolingBasic CoolingCoil => this.Children.Get<IB_CoilCoolingBasic>();
        private IB_CoilHeatingBasic HeatingCoil => this.Children.Get<IB_CoilHeatingBasic>();
        private IB_Fan Fan => this.Children.Get<IB_Fan>();

        public IB_ZoneHVACPackagedTerminalAirConditioner(IB_Fan SupplyFan, IB_CoilHeatingBasic HeatingCoil, IB_CoilCoolingBasic CoolingCoil) 
            : base(NewDefaultOpsObj(new Model(),SupplyFan, HeatingCoil, CoolingCoil))
        {
            this.AddChild(CoolingCoil);
            this.AddChild(HeatingCoil);
            this.AddChild(SupplyFan);
        }
        public void SetFan(IB_Fan Fan)
        {
            this.SetChild(Fan);
        }
        
        public void SetCoolingCoil(IB_CoilCoolingBasic Coil)
        {
            this.SetChild(Coil);
        }
        public void SetHeatingCoil(IB_CoilHeatingBasic Coil)
        {
            this.SetChild(Coil);
        }

        protected override ModelObject NewOpsObj(Model model)
        {
            var opsObj =  base.OnNewOpsObj(LocalInitMethod, model).to_ZoneHVACUnitVentilator().get();
            return opsObj;

            ZoneHVACPackagedTerminalAirConditioner LocalInitMethod(Model m)
            => new ZoneHVACPackagedTerminalAirConditioner(
                m,
                m.alwaysOnDiscreteSchedule(),
                Fan.ToOS(m),
                HeatingCoil.ToOS(m),
                CoolingCoil.ToOS(m)
                );

        }
    }

    public sealed class IB_ZoneHVACPackagedTerminalAirConditioner_DataFieldSet
        : IB_FieldSet<IB_ZoneHVACPackagedTerminalAirConditioner_DataFieldSet, ZoneHVACPackagedTerminalAirConditioner>
    {
        private IB_ZoneHVACPackagedTerminalAirConditioner_DataFieldSet() {}

    }
}
