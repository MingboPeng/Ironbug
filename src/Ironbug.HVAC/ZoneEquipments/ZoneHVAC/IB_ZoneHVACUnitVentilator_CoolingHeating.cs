using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACUnitVentilator_CoolingHeating : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACUnitVentilator_CoolingHeating();

        private static ZoneHVACUnitVentilator InitMethod(Model model) => new ZoneHVACUnitVentilator(model);
        private IB_CoilCoolingBasic CoolingCoil => this.Children.Get<IB_CoilCoolingBasic>();
        private IB_CoilHeatingBasic HeatingCoil => this.Children.Get<IB_CoilHeatingBasic>();
        private IB_Fan Fan => this.Children.Get<IB_Fan>();

        public IB_ZoneHVACUnitVentilator_CoolingHeating() : base(InitMethod(new Model()))
        {

            this.AddChild(new IB_CoilCoolingWater());
            this.AddChild(new IB_CoilHeatingWater());
            this.AddChild(new IB_FanConstantVolume());

        }
        public void SetFan(IB_Fan Fan)
        {
            this.SetChild(Fan);
        }

        //TODO: This is only for testing, need to be removed.
        public IB_Fan GetFan()
        {
           return this.Children.Get<IB_Fan>();
        }
        
        public void SetCoolingCoil(IB_CoilCoolingBasic Coil)
        {
            this.SetChild(Coil);
        }
        public void SetHeatingCoil(IB_CoilHeatingBasic Coil)
        {
            this.SetChild(Coil);
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            var opsObj =  base.OnInitOpsObj(InitMethod, model).to_ZoneHVACUnitVentilator().get();
            opsObj.setCoolingCoil(this.CoolingCoil.ToOS(model));
            opsObj.setHeatingCoil(this.HeatingCoil.ToOS(model));
            opsObj.setSupplyAirFan(this.Fan.ToOS(model));
            return opsObj;
            
        }
    }

    public sealed class IB_ZoneHVACUnitVentilator_DataFieldSet 
        : IB_FieldSet<IB_ZoneHVACUnitVentilator_DataFieldSet, ZoneHVACUnitVentilator>
    {
        private IB_ZoneHVACUnitVentilator_DataFieldSet() {}

    }
}
