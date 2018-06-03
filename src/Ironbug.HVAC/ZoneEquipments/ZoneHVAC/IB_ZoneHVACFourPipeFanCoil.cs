using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACFourPipeFanCoil : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACFourPipeFanCoil();

        private static ZoneHVACFourPipeFanCoil InitMethod(Model model)
            => new ZoneHVACFourPipeFanCoil(model, model.alwaysOnDiscreteSchedule(), new FanConstantVolume(model), new CoilCoolingWater(model), new CoilHeatingWater(model));

        private IB_Child CoolingCoil => this.Children.GetChild<IB_CoilCoolingWater>();
        private IB_Child HeatingCoil => this.Children.GetChild<IB_CoilHeatingWater>();
        private IB_Child Fan => this.Children.GetChild<IB_Fan>();

        public IB_ZoneHVACFourPipeFanCoil() : base(InitMethod(new Model()))
        {
            var hCoil = new IB_Child(new IB_CoilHeatingWater(), (obj) => this.SetHeatingCoil(obj as IB_CoilHeatingWater));
            var cCoil = new IB_Child(new IB_CoilCoolingWater(), (obj) => this.SetCoolingCoil(obj as IB_CoilCoolingWater));
            var fan = new IB_Child(new IB_FanConstantVolume(), (obj) => this.SetFan(obj as IB_Fan));
            this.Children.Add(hCoil);
            this.Children.Add(cCoil);
            this.Children.Add(fan);
        }

        public void SetFan(IB_Fan Fan)
        {
            this.Fan.Set(Fan);
        }

        public void SetHeatingCoil(IB_CoilHeatingWater Coil)
        {
            this.HeatingCoil.Set(Coil);
        }

        public void SetCoolingCoil(IB_CoilCoolingWater Coil)
        {
            this.CoolingCoil.Set(Coil);
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            var opsObj = base.OnInitOpsObj(InitMethod, model).to_ZoneHVACFourPipeFanCoil().get();
            opsObj.setCoolingCoil((HVACComponent)this.CoolingCoil.To<IB_CoilCoolingWater>().ToOS(model));
            opsObj.setHeatingCoil((HVACComponent)this.HeatingCoil.To<IB_CoilHeatingWater>().ToOS(model));
            opsObj.setSupplyAirFan((HVACComponent)this.Fan.To<IB_Fan>().ToOS(model));
            return opsObj;
        }
    }

    public sealed class IB_ZoneHVACFourPipeFanCoil_DataFieldSet
        : IB_DataFieldSet<IB_ZoneHVACFourPipeFanCoil_DataFieldSet, ZoneHVACFourPipeFanCoil>
    {
        private IB_ZoneHVACFourPipeFanCoil_DataFieldSet() {}
        public IB_DataField CapacityControlMethod { get; }
            = new IB_BasicDataField("CapacityControlMethod", "CapacityCtrl");
        
    }
}