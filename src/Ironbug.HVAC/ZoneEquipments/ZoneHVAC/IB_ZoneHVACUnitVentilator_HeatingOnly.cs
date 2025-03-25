using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACUnitVentilator_HeatingOnly : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACUnitVentilator_HeatingOnly();

        private static ZoneHVACUnitVentilator NewDefaultOpsObj(Model model) => new ZoneHVACUnitVentilator(model);
        private IB_CoilBasic HeatingCoil => this.GetChild<IB_CoilHeatingBasic>();
        private IB_Fan Fan => this.GetChild<IB_Fan>();

        [JsonConstructor]
        private IB_ZoneHVACUnitVentilator_HeatingOnly(bool forDeserialization) : base(null)
        {
        }
        public IB_ZoneHVACUnitVentilator_HeatingOnly() : base(NewDefaultOpsObj)
        {
            this.AddChild(new IB_CoilHeatingWater());
            this.AddChild(new IB_FanConstantVolume());
        }
        public void SetFan(IB_Fan Fan)
        {
            this.SetChild(Fan);
        }
        
        public void SetHeatingCoil(IB_CoilHeatingBasic Coil)
        {
            this.SetChild(Coil);
        }

        public override HVACComponent ToOS(Model model)
        {
            var opsObj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            opsObj.setHeatingCoil(this.HeatingCoil.ToOS(model));
            opsObj.setSupplyAirFan(this.Fan.ToOS(model));
            return opsObj;
        }
        
    }
    
}
