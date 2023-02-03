﻿using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACUnitVentilator_CoolingHeating : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACUnitVentilator_CoolingHeating();

        private static ZoneHVACUnitVentilator NewDefaultOpsObj(Model model) => new ZoneHVACUnitVentilator(model);
        private IB_CoilCoolingBasic CoolingCoil => this.GetChild<IB_CoilCoolingBasic>();
        private IB_CoilHeatingBasic HeatingCoil => this.GetChild<IB_CoilHeatingBasic>();
        private IB_Fan Fan => this.GetChild<IB_Fan>();

        [JsonConstructor]
        private IB_ZoneHVACUnitVentilator_CoolingHeating(bool forDeserialization) : base(null)
        {
        }
        public IB_ZoneHVACUnitVentilator_CoolingHeating() : base(NewDefaultOpsObj(new Model()))
        {

            this.AddChild(new IB_CoilCoolingWater());
            this.AddChild(new IB_CoilHeatingWater());
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
        public void SetHeatingCoil(IB_CoilHeatingBasic Coil)
        {
            this.SetChild(Coil);
        }

        public override HVACComponent ToOS(Model model)
        {
            var opsObj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            opsObj.setCoolingCoil(this.CoolingCoil.ToOS(model));
            opsObj.setHeatingCoil(this.HeatingCoil.ToOS(model));
            opsObj.setSupplyAirFan(this.Fan.ToOS(model));
            return opsObj;
        }
        
    }

    public sealed class IB_ZoneHVACUnitVentilator_FieldSet 
        : IB_FieldSet<IB_ZoneHVACUnitVentilator_FieldSet, ZoneHVACUnitVentilator>
    {
        private IB_ZoneHVACUnitVentilator_FieldSet() {}

    }
}
