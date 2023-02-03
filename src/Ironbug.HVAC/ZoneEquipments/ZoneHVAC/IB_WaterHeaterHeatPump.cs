using System;
using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_WaterHeaterHeatPump : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_WaterHeaterHeatPump();

        private static WaterHeaterHeatPump NewDefaultOpsObj(Model model) => new WaterHeaterHeatPump(model);

        private IB_WaterHeaterMixed _waterHeater => this.GetChild<IB_WaterHeaterMixed>();
        private IB_CoilWaterHeatingAirToWaterHeatPump _heatingCoil => this.GetChild<IB_CoilWaterHeatingAirToWaterHeatPump>();
        private IB_FanOnOff _fan => this.GetChild<IB_FanOnOff>();

        [JsonConstructor]
        private IB_WaterHeaterHeatPump(bool forDeserialization) : base(null)
        {
        }

        public IB_WaterHeaterHeatPump() : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(new IB_WaterHeaterMixed());
            this.AddChild(new IB_CoilWaterHeatingAirToWaterHeatPump());
            this.AddChild(new IB_FanOnOff());
        }

        public void SetTank(IB_WaterHeaterMixed Tank)
        {
            this.SetChild(0, Tank);
        }

        public void SetHeatingCoil(IB_CoilWaterHeatingAirToWaterHeatPump HeatingCoil)
        {
            this.SetChild(1, HeatingCoil);
        }

        public void SetFan(IB_FanOnOff Fan)
        {
            this.SetChild(2, Fan);
        }

        public override HVACComponent ToOS(Model model)
        {
            var opsObj = base.OnNewOpsObj(NewDefaultOpsObj, model);

            if (this._waterHeater != null) opsObj.setTank(this._waterHeater.ToOS(model));
            if (this._heatingCoil != null) opsObj.setDXCoil(this._heatingCoil.ToOS(model));
            if (this._fan != null) opsObj.setFan(this._fan.ToOS(model));

            return opsObj;

        }

    }


    public sealed class IB_WaterHeaterHeatPump_FieldSet
        : IB_FieldSet<IB_WaterHeaterHeatPump_FieldSet, WaterHeaterHeatPump>
    {
        private IB_WaterHeaterHeatPump_FieldSet() { }
    }
}
