using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACBaseboardConvectiveWater : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACBaseboardConvectiveWater();

        private static ZoneHVACBaseboardConvectiveWater InitMethod(Model model) 
            => new ZoneHVACBaseboardConvectiveWater(model, model.alwaysOnDiscreteSchedule(), new CoilHeatingWaterBaseboard(model));

        private IB_CoilHeatingWaterBaseboard HeatingCoil => this.Children.Get<IB_CoilHeatingWaterBaseboard>();

        public IB_ZoneHVACBaseboardConvectiveWater() : base(InitMethod(new Model()))
        {
            this.AddChild(new IB_CoilHeatingWaterBaseboard());
        }

        public void SetHeatingCoil(IB_CoilHeatingWaterBaseboard Coil)
        {
            this.SetChild(Coil);
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethodWithChildren, model).to_ZoneHVACBaseboardConvectiveElectric().get();
            //Local Method
            ZoneHVACBaseboardConvectiveWater InitMethodWithChildren(Model md) =>
                new ZoneHVACBaseboardConvectiveWater(md, md.alwaysOnDiscreteSchedule(), (StraightComponent)this.HeatingCoil.ToOS(md));
        }
    }

    public sealed class IB_ZoneHVACBaseboardConvectiveWater_DataFieldSet
        : IB_FieldSet<IB_ZoneHVACBaseboardConvectiveWater_DataFieldSet, ZoneHVACBaseboardConvectiveWater>
    {
        private IB_ZoneHVACBaseboardConvectiveWater_DataFieldSet() { }

    }
}
