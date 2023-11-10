using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_AirLoopHVACUnitarySystem : IB_AirLoopHVACUnitary, IIB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () 
            => new IB_AirLoopHVACUnitarySystem();

        private static AirLoopHVACUnitarySystem NewDefaultOpsObj(Model m) 
            => new AirLoopHVACUnitarySystem(m);

        private IB_Coil _coolingCoil => this.GetChild<IB_Coil>(0);
        private IB_Coil _heatingCoil => this.GetChild<IB_Coil>(1);
        private IB_Fan _fan => this.GetChild<IB_Fan>(2);
        private IB_Coil _supplementalHeatingCoil => this.GetChild<IB_Coil>(3);
        private string _controlZoneName { get => this.Get(string.Empty); set => this.Set(value); }

        [JsonConstructor]
        private IB_AirLoopHVACUnitarySystem(bool forDeserialization) : base(null)
        {
        }

        public IB_AirLoopHVACUnitarySystem() 
            : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(null);
            this.AddChild(null);
            this.AddChild(null);
            this.AddChild(null);
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

        public void SetControlZone(string controlZoneName)
        {
            if (string.IsNullOrEmpty(controlZoneName))
                throw new ArgumentException("Invalid control zone");
            _controlZoneName = controlZoneName;
        }

        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);

            if (this._coolingCoil != null) obj.setCoolingCoil(this._coolingCoil.ToOS(model));
            if (this._heatingCoil != null) obj.setHeatingCoil(this._heatingCoil.ToOS(model));
            if (this._fan != null) obj.setSupplyFan(this._fan.ToOS(model));
            if (this._supplementalHeatingCoil != null) obj.setSupplementalHeatingCoil(this._supplementalHeatingCoil.ToOS(model));

            if (!string.IsNullOrEmpty(_controlZoneName))
            {
                // this will be executed after all loops (nodes) are saved
                Func<bool> func = () =>
                {
                    var zone = model.GetThermalZone(_controlZoneName);
                    if (zone == null)
                        return false;

                    return obj.setControllingZoneorThermostatLocation(zone);

                };

                IB_Utility.AddDelayFunc(func);
            }

            return obj;

        }
    }

    public sealed class IB_AirLoopHVACUnitarySystem_FieldSet
        : IB_FieldSet<IB_AirLoopHVACUnitarySystem_FieldSet, AirLoopHVACUnitarySystem>
    {
        private IB_AirLoopHVACUnitarySystem_FieldSet() {}

    }
}
