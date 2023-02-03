using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACUnitVentilator_CoolingOnly : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACUnitVentilator_CoolingOnly();

        private static ZoneHVACUnitVentilator NewDefaultOpsObj(Model model) => new ZoneHVACUnitVentilator(model);
        private IB_CoilBasic CoolingCoil => this.GetChild<IB_CoilCoolingBasic>();
        private IB_Fan Fan => this.GetChild<IB_Fan>();

        [JsonConstructor]
        private IB_ZoneHVACUnitVentilator_CoolingOnly(bool forDeserialization) : base(null)
        {
        }
        public IB_ZoneHVACUnitVentilator_CoolingOnly() : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(new IB_CoilCoolingWater());
            this.AddChild(new IB_FanConstantVolume());
        }
        public void SetFan(IB_Fan Fan)
        {
            this.SetChild(Fan);
        }
        
        public void SetCoolingCoil(IB_CoilCoolingBasic Coil)
        {
            this.SetChild(Coil);
        }

        public override HVACComponent ToOS(Model model)
        {
            var opsObj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            opsObj.setCoolingCoil(this.CoolingCoil.ToOS(model));
            opsObj.setSupplyAirFan(this.Fan.ToOS(model));
            return opsObj;
        }
    }
    
}
