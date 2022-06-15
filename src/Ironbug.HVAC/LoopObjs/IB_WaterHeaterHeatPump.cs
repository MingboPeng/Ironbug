using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_WaterHeaterHeatPump : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_WaterHeaterHeatPump();

        private static WaterHeaterHeatPump NewDefaultOpsObj(Model model) => new WaterHeaterHeatPump(model);

        private IB_Coil _coolingCoil => this.GetChild<IB_Coil>(0);
        private IB_Coil _heatingCoil => this.GetChild<IB_Coil>(1);
        private IB_Fan _fan => this.GetChild<IB_Fan>(2);

        public IB_WaterHeaterHeatPump() : base(NewDefaultOpsObj(new Model()))
        {
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

        public override HVACComponent ToOS(Model model)
        {
            var opsObj = base.OnNewOpsObj(NewDefaultOpsObj, model);

            if (this._coolingCoil != null) opsObj.setCoolingCoil(this._coolingCoil.ToOS(model));
            if (this._heatingCoil != null) opsObj.setHeatingCoil(this._heatingCoil.ToOS(model));
            if (this._fan != null) opsObj.setSupplyFan(this._fan.ToOS(model));

            return opsObj;

        }

    }


    public sealed class IB_WaterHeaterHeatPump_FieldSet
        : IB_FieldSet<IB_WaterHeaterHeatPump_FieldSet, WaterHeaterHeatPump>
    {
        private IB_WaterHeaterHeatPump_FieldSet() { }
    }
}
