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

        private IB_CoilCoolingWater CoolingCoil => this.Children.Get<IB_CoilCoolingWater>();
        private IB_CoilHeatingWater HeatingCoil => this.Children.Get<IB_CoilHeatingWater>();
        private IB_Fan Fan => this.Children.Get<IB_Fan>();

        public IB_ZoneHVACFourPipeFanCoil() : base(InitMethod(new Model()))
        {
            this.AddChild(new IB_CoilHeatingWater());
            this.AddChild(new IB_CoilCoolingWater());
            this.AddChild(new IB_FanConstantVolume());
        }

        public void SetFan(IB_Fan Fan)
        {
            this.SetChild(Fan);
        }

        public void SetHeatingCoil(IB_CoilHeatingWater Coil)
        {
            this.SetChild(Coil);
        }

        public void SetCoolingCoil(IB_CoilCoolingWater Coil)
        {
            this.SetChild(Coil);
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            var opsObj = base.OnInitOpsObj(LocalInitMethod, model).to_ZoneHVACFourPipeFanCoil().get();
            return opsObj;

            ZoneHVACFourPipeFanCoil LocalInitMethod(Model md)
            => new ZoneHVACFourPipeFanCoil(md, md.alwaysOnDiscreteSchedule(),
            this.Fan.ToOS(md),
            this.CoolingCoil.ToOS(md),
            this.HeatingCoil.ToOS(md)
            );

        }
    }

    public sealed class IB_ZoneHVACFourPipeFanCoil_DataFieldSet
        : IB_FieldSet<IB_ZoneHVACFourPipeFanCoil_DataFieldSet, ZoneHVACFourPipeFanCoil>
    {
        private IB_ZoneHVACFourPipeFanCoil_DataFieldSet() {}
        public IB_Field CapacityControlMethod { get; }
            = new IB_BasicField("CapacityControlMethod", "CapacityCtrl");
        
    }
}