using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_AirLoopHVACUnitarySystem : IB_HVACObject, IIB_AirLoopObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () 
            => new IB_AirLoopHVACUnitarySystem();

        private static AirLoopHVACUnitarySystem NewDefaultOpsObj(Model m) 
            => new AirLoopHVACUnitarySystem(m);

        private IB_Coil _coolingCoil => this.Children.Get<IB_Coil>(0);
        private IB_Coil _heatingCoil => this.Children.Get<IB_Coil>(1);
        private IB_Fan _fan => this.Children.Get<IB_Fan>(2);
        private IB_Coil _supplementalHeatingCoil => this.Children.Get<IB_Coil>(3);

        public IB_AirLoopHVACUnitarySystem() 
            : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(null);
            this.AddChild(null);
            this.AddChild(new IB_FanOnOff());
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

        public override HVACComponent ToOS(Model model)
        {
            var opsObj = base.OnNewOpsObj(NewDefaultOpsObj, model);

            if (this._coolingCoil != null) opsObj.setCoolingCoil(this._coolingCoil.ToOS(model));
            if (this._heatingCoil != null) opsObj.setHeatingCoil(this._heatingCoil.ToOS(model));
            if (this._fan != null) opsObj.setSupplyFan(this._fan.ToOS(model));
            if (this._supplementalHeatingCoil != null) opsObj.setSupplementalHeatingCoil(this._supplementalHeatingCoil.ToOS(model));

            return opsObj;

        }
    }

    public sealed class IB_AirLoopHVACUnitarySystem_FieldSet
        : IB_FieldSet<IB_AirLoopHVACUnitarySystem_FieldSet, AirLoopHVACUnitarySystem>
    {
        private IB_AirLoopHVACUnitarySystem_FieldSet() {}

    }
}
