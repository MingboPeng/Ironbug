using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_AirLoopHVACUnitaryHeatPumpAirToAir : IB_AirLoopHVACUnitary
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () 
            => new IB_AirLoopHVACUnitaryHeatPumpAirToAir();

        private static AirLoopHVACUnitaryHeatPumpAirToAir NewDefaultOpsObj(Model m)
        {
            var sch = new ScheduleRuleset(m);
            var fan = new FanConstantVolume(m);
            var cH = new CoilHeatingDXSingleSpeed(m);
            var cC = new CoilCoolingDXSingleSpeed(m);
            var sCH = new CoilHeatingElectric(m);
            var obj = new AirLoopHVACUnitaryHeatPumpAirToAir(m, sch, fan, cH, cC, sCH);
            return obj;
        }

        private IB_CoilDX _coolingCoil => this.GetChild<IB_CoilDX>(0);
        private IB_CoilDX _heatingCoil => this.GetChild<IB_CoilDX>(1); 
        private IB_Fan _fan => this.GetChild<IB_Fan>(2);
        private IB_CoilHeatingBasic _supplementalHeatingCoil => this.GetChild<IB_CoilHeatingBasic>(3);
        private string _controlZoneName { get => this.Get(string.Empty); set => this.Set(value); }

        [JsonConstructor]
        private IB_AirLoopHVACUnitaryHeatPumpAirToAir(bool forDeserialization) : base(null)
        {
        }

        public IB_AirLoopHVACUnitaryHeatPumpAirToAir() 
            : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(new IB_CoilCoolingDXSingleSpeed());
            this.AddChild(new IB_CoilHeatingDXSingleSpeed());
            this.AddChild(new IB_FanConstantVolume());
            this.AddChild(new IB_CoilHeatingElectric());
        }


        public void SetCoolingCoil(IB_CoilDX coolingCoil)
        {
            this.SetChild(0, coolingCoil);
        }

        public void SetHeatingCoil(IB_CoilDX heatingCoil)
        {
            this.SetChild(1, heatingCoil);
        }

        public void SetFan(IB_Fan Fan)
        {
            this.SetChild(2, Fan);
        }

        public void SetSupplementalHeatingCoil(IB_CoilHeatingBasic heatingCoil)
        {
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
                        return false;

                    return obj.setControllingZone(zone);

                };

                IB_Utility.AddDelayFunc(func);
            }

            return obj;

        }
    }

    public sealed class IB_AirLoopHVACUnitaryHeatPumpAirToAir_FieldSet
        : IB_FieldSet<IB_AirLoopHVACUnitaryHeatPumpAirToAir_FieldSet, AirLoopHVACUnitaryHeatPumpAirToAir>
    {
        private IB_AirLoopHVACUnitaryHeatPumpAirToAir_FieldSet() {}

    }
}
