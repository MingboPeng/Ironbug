using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACUnitVentilator_CoolingHeating : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACUnitVentilator_CoolingHeating();

        private static ZoneHVACUnitVentilator InitMethod(Model model) => new ZoneHVACUnitVentilator(model);
        private IB_Child CoolingCoil => this.Children.GetChild<IB_CoilBasic>();
        private IB_Child HeatingCoil => this.Children.GetChild<IB_CoilBasic>();
        private IB_Child Fan => this.Children.GetChild<IB_Fan>();

        public IB_ZoneHVACUnitVentilator_CoolingHeating() : base(InitMethod(new Model()))
        {
            var coolingCoil = new IB_Child(new IB_CoilCoolingWater(), (obj) => this.SetCoolingCoil(obj as IB_CoilBasic));
            var heatingCoil = new IB_Child(new IB_CoilHeatingWater(), (obj) => this.SetHeatingCoil(obj as IB_CoilBasic));
            var fan = new IB_Child(new IB_FanConstantVolume(), (obj) => this.SetFan(obj as IB_Fan));

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
            var opsObj =  base.OnInitOpsObj(InitMethod, model).to_ZoneHVACUnitVentilator().get();
            opsObj.setCoolingCoil((HVACComponent)this.CoolingCoil.To<IB_CoilBasic>().ToOS(model));
            opsObj.setHeatingCoil((HVACComponent)this.HeatingCoil.To<IB_CoilBasic>().ToOS(model));
            opsObj.setSupplyAirFan((HVACComponent)this.Fan.To<IB_Fan>().ToOS(model));
            return opsObj;
            
        }
    }

    public sealed class IB_ZoneHVACUnitVentilator_DataFieldSet 
        : IB_FieldSet<IB_ZoneHVACUnitVentilator_DataFieldSet, ZoneHVACUnitVentilator>
    {
        private IB_ZoneHVACUnitVentilator_DataFieldSet() {}

    }
}
