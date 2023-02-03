﻿using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_AirLoopHVACUnitarySystem : IB_HVACObject, IIB_AirLoopObject, IIB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () 
            => new IB_AirLoopHVACUnitarySystem(this._Zone);

        private static AirLoopHVACUnitarySystem NewDefaultOpsObj(Model m) 
            => new AirLoopHVACUnitarySystem(m);

        private IB_Coil _coolingCoil => this.GetChild<IB_Coil>(0);
        private IB_Coil _heatingCoil => this.GetChild<IB_Coil>(1);
        private IB_Fan _fan => this.GetChild<IB_Fan>(2);
        private IB_Coil _supplementalHeatingCoil => this.GetChild<IB_Coil>(3);
        private IB_ThermalZone _Zone => this.GetChild<IB_ThermalZone>();

        [JsonConstructor]
        private IB_AirLoopHVACUnitarySystem(bool forDeserialization) : base(null)
        {
        }

        public IB_AirLoopHVACUnitarySystem(IB_ThermalZone ControllingZone) 
            : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(null);
            this.AddChild(null);
            this.AddChild(null);
            this.AddChild(null);
            this.AddChild(ControllingZone);
        }

        //constructor for no controlling zone option
        public IB_AirLoopHVACUnitarySystem()
            : this(null)
        {
        }

        public void SetCoolingCoil(IB_Coil CoolingCoil)
        {
            this.SetChild(0, CoolingCoil);
        }

        public void SetHeatingCoil(IB_Coil HeatingCoil)
        {
            this.SetChild(1, HeatingCoil);
        }

        public void SetFan(IB_Fan Fan)
        {
            this.SetChild(2, Fan);
        }

        public void SetSupplementalHeatingCoil(IB_Coil HeatingCoil)
        {
            this.SetChild(3, HeatingCoil);
        }
        public void SetControllingZone(IB_ThermalZone Zone)
        {
            this.SetChild(4, Zone);
        }

        public override HVACComponent ToOS(Model model)
        {
            var opsObj = base.OnNewOpsObj(NewDefaultOpsObj, model);

            if (this._coolingCoil != null) opsObj.setCoolingCoil(this._coolingCoil.ToOS(model));
            if (this._heatingCoil != null) opsObj.setHeatingCoil(this._heatingCoil.ToOS(model));
            if (this._fan != null) opsObj.setSupplyFan(this._fan.ToOS(model));
            if (this._supplementalHeatingCoil != null) opsObj.setSupplementalHeatingCoil(this._supplementalHeatingCoil.ToOS(model));

            if (this._Zone != null)
                opsObj.setControllingZoneorThermostatLocation(this._Zone.ToOS(model) as ThermalZone);

            return opsObj;

        }
    }

    public sealed class IB_AirLoopHVACUnitarySystem_FieldSet
        : IB_FieldSet<IB_AirLoopHVACUnitarySystem_FieldSet, AirLoopHVACUnitarySystem>
    {
        private IB_AirLoopHVACUnitarySystem_FieldSet() {}

    }
}
