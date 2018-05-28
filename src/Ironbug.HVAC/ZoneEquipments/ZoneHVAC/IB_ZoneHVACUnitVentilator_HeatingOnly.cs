using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACUnitVentilator_HeatingOnly : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACUnitVentilator_HeatingOnly();

        private static ZoneHVACUnitVentilator InitMethod(Model model) => new ZoneHVACUnitVentilator(model);
        private IB_Child HeatingCoil => this.Children.GetChild<IB_CoilBasic>();
        private IB_Child Fan => this.Children.GetChild<IB_Fan>();

        public IB_ZoneHVACUnitVentilator_HeatingOnly() : base(InitMethod(new Model()))
        {
            var heatingCoil = new IB_Child(new IB_CoilHeatingWater(), (obj) => this.SetHeatingCoil(obj as IB_CoilBasic));
            var fan = new IB_Child(new IB_FanConstantVolume(), (obj) => this.SetFan(obj as IB_Fan));
            
            this.Children.Add(heatingCoil);
            this.Children.Add(fan);
        }
        public void SetFan(IB_Fan Fan)
        {
            this.Fan.Set(Fan);
        }
        
        public void SetHeatingCoil(IB_CoilBasic Coil)
        {
            this.HeatingCoil.Set(Coil);
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            var opsObj =  base.OnInitOpsObj(InitMethod, model).to_ZoneHVACUnitVentilator().get();
            opsObj.setHeatingCoil((HVACComponent)this.HeatingCoil.To<IB_CoilBasic>().ToOS(model));
            opsObj.setSupplyAirFan((HVACComponent)this.Fan.To<IB_Fan>().ToOS(model));
            return opsObj;
            
        }
    }
    
}
