using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_WaterHeaterHeatPump : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_WaterHeaterHeatPump();

        private static WaterHeaterHeatPump NewDefaultOpsObj(Model model) => new WaterHeaterHeatPump(model);

        private IB_WaterHeaterMixed _waterHeater => this.GetChild<IB_WaterHeaterMixed>(0);
        private IB_Coil _heatingCoil => this.GetChild<IB_Coil>(1);
        private IB_Fan _fan => this.GetChild<IB_Fan>(2);

        public IB_WaterHeaterHeatPump() : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(null);
            this.AddChild(null);
            this.AddChild(null);
        }

        public void SetTank(IB_WaterHeaterMixed Tank)
        {
            this.SetChild(0, Tank);
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
