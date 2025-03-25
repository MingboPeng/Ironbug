using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed : IB_AirLoopHVACUnitary
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () 
            => new IB_AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed();

        private static AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed NewDefaultOpsObj(Model m)
        {
            var fan = new FanConstantVolume(m);
            var cH = new CoilHeatingDXMultiSpeed(m);
            var cC = new CoilCoolingDXMultiSpeed(m);
            var sCH = new CoilHeatingElectric(m);
            var obj = new AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed(m, fan, cH, cC, sCH);
            return obj;
        }

        private IB_Coil _coolingCoil => this.GetChild<IB_Coil>(0);
        private IB_Coil _heatingCoil => this.GetChild<IB_Coil>(1); // DX heating coil or Multi-stage gas coil
        private IB_Fan _fan => this.GetChild<IB_Fan>(2);
        private IB_CoilHeatingBasic _supplementalHeatingCoil => this.GetChild<IB_CoilHeatingBasic>(3);
        private string _controlZoneName { get => this.Get(string.Empty); set => this.Set(value); }

        [JsonConstructor]
        private IB_AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed(bool forDeserialization) : base(null)
        {
        }


        public IB_AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed() 
            : base(NewDefaultOpsObj)
        {
            this.AddChild(new IB_CoilCoolingDXMultiSpeed());
            this.AddChild(new IB_CoilHeatingDXMultiSpeed());
            this.AddChild(new IB_FanConstantVolume());
            this.AddChild(new IB_CoilHeatingElectric());
        }


        public void SetCoolingCoil(IB_Coil coolingCoil)
        {
            // test if obj is valid
            var ghostModel = this.GhostOSModel;
            if (!(this.GhostOSObject as AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed).setCoolingCoil(coolingCoil.ToOS(ghostModel)))
                throw new ArgumentException("Invalid cooling coil!");

            this.SetChild(0, coolingCoil);
        }

        public void SetHeatingCoil(IB_Coil heatingCoil)
        { 
            // test if obj is valid
            var ghostModel = this.GhostOSModel;
            if (!(this.GhostOSObject as AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed).setHeatingCoil(heatingCoil.ToOS(ghostModel)))
                throw new ArgumentException("Invalid heating coil!");

            this.SetChild(1, heatingCoil);
        }

        public void SetFan(IB_Fan fan)
        {
            // test if obj is valid
            var ghostModel = this.GhostOSModel;
            if (!(this.GhostOSObject as AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed).setSupplyAirFan(fan.ToOS(ghostModel)))
                throw new ArgumentException("Invalid supply fan!");

            this.SetChild(2, fan);
        }

        public void SetSupplementalHeatingCoil(IB_CoilHeatingBasic heatingCoil)
        {
            // test if obj is valid
            var ghostModel = this.GhostOSModel;
            if (!(this.GhostOSObject as AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed).setSupplementalHeatingCoil(heatingCoil.ToOS(ghostModel)))
                throw new ArgumentException("Invalid SupplementalHeatingCoil!");


            this.SetChild(3, heatingCoil);
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
            if (this._fan != null) obj.setSupplyAirFan(this._fan.ToOS(model));
            if (this._supplementalHeatingCoil != null) obj.setSupplementalHeatingCoil(this._supplementalHeatingCoil.ToOS(model));

            if (!string.IsNullOrEmpty(_controlZoneName))
            {
                // this will be executed after all loops (nodes) are saved
                Func<bool> func = () =>
                {
                    var zone = model.GetThermalZone(_controlZoneName);
                    if (zone == null)
                        throw new ArgumentException($"Invalid control zone ({_controlZoneName}) in {this.GetType().Name}");

                    return obj.setControllingZoneorThermostatLocation(zone);

                };

                IB_Utility.AddDelayFunc(func);
            }

            return obj;

        }
    }

    public sealed class IB_AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed_FieldSet
        : IB_FieldSet<IB_AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed_FieldSet, AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed>
    {
        private IB_AirLoopHVACUnitaryHeatPumpAirToAirMultiSpeed_FieldSet() {}

    }
}
