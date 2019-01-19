using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACPackagedTerminalAirConditioner : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACPackagedTerminalAirConditioner(Fan.To<IB_Fan>(), CoolingCoil.To<IB_CoilBasic>(), HeatingCoil.To<IB_CoilBasic>());

        private static ZoneHVACPackagedTerminalAirConditioner InitMethod(Model model, IB_Fan SupplyFan, IB_CoilBasic HeatingCoil, IB_CoilBasic CoolingCoil) 
            => new ZoneHVACPackagedTerminalAirConditioner(model,model.alwaysOnDiscreteSchedule(), SupplyFan.ToOS(model), HeatingCoil.ToOS(model), CoolingCoil.ToOS(model));

        private IB_Child CoolingCoil => this.Children.GetChild<IB_CoilBasic>();
        private IB_Child HeatingCoil => this.Children.GetChild<IB_CoilBasic>();
        private IB_Child Fan => this.Children.GetChild<IB_Fan>();

        public IB_ZoneHVACPackagedTerminalAirConditioner(IB_Fan SupplyFan, IB_CoilBasic HeatingCoil, IB_CoilBasic CoolingCoil) 
            : base(InitMethod(new Model(),SupplyFan, HeatingCoil, CoolingCoil))
        {
            var coolingCoil = new IB_Child(CoolingCoil, (obj) => this.SetCoolingCoil(obj as IB_CoilBasic));
            var heatingCoil = new IB_Child(HeatingCoil, (obj) => this.SetHeatingCoil(obj as IB_CoilBasic));
            var fan = new IB_Child(SupplyFan, (obj) => this.SetFan(obj as IB_Fan)); 

            this.Children.Add(coolingCoil);
            this.Children.Add(heatingCoil);
            this.Children.Add(fan);
        }
        public void SetFan(IB_Fan Fan)
        {
            this.Fan.Set(Fan);
        }
        
        public void SetCoolingCoil(IB_CoilBasic Coil)
        {
            this.CoolingCoil.Set(Coil);
        }
        public void SetHeatingCoil(IB_CoilBasic Coil)
        {
            this.HeatingCoil.Set(Coil);
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            var opsObj =  base.OnInitOpsObj(LocalInitMethod, model).to_ZoneHVACUnitVentilator().get();
            return opsObj;

            ZoneHVACPackagedTerminalAirConditioner LocalInitMethod(Model m)
            => new ZoneHVACPackagedTerminalAirConditioner(
                m,
                m.alwaysOnDiscreteSchedule(),
                Fan.To<IB_Fan>().ToOS(m),
                HeatingCoil.To<IB_CoilBasic>().ToOS(m),
                CoolingCoil.To<IB_CoilBasic>().ToOS(m)
                );

        }
    }

    public sealed class IB_ZoneHVACPackagedTerminalAirConditioner_DataFieldSet
        : IB_FieldSet<IB_ZoneHVACPackagedTerminalAirConditioner_DataFieldSet, ZoneHVACPackagedTerminalAirConditioner>
    {
        private IB_ZoneHVACPackagedTerminalAirConditioner_DataFieldSet() {}

    }
}
