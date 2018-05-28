using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACBaseboardRadiantConvectiveWater : BaseClass.IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACBaseboardRadiantConvectiveWater();

        private static ZoneHVACBaseboardRadiantConvectiveWater InitMethod(Model model) 
            => new ZoneHVACBaseboardRadiantConvectiveWater(model);

        private IB_Child HeatingCoil => this.Children.GetChild<IB_CoilHeatingWaterBaseboardRadiant>();

        public IB_ZoneHVACBaseboardRadiantConvectiveWater() : base(InitMethod(new Model()))
        {
            var heatingCoil = new IB_Child(new IB_CoilHeatingWaterBaseboardRadiant(), (obj) => this.SetHeatingCoil(obj as IB_CoilHeatingWaterBaseboardRadiant));
            this.Children.Add(heatingCoil);
        }

        public void SetHeatingCoil(IB_CoilHeatingWaterBaseboardRadiant Coil)
        {
            this.HeatingCoil.Set(Coil);
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            var opsObj =  base.OnInitOpsObj(InitMethod, model).to_ZoneHVACBaseboardRadiantConvectiveWater().get();
            opsObj.setHeatingCoil((HVACComponent)this.HeatingCoil.To<IB_CoilHeatingWaterBaseboardRadiant>().ToOS(model));
            return opsObj;
        }
    }

    public sealed class IB_ZoneHVACBaseboardRadiantConvectiveWater_DataFieldSet
        : IB_DataFieldSet<IB_ZoneHVACBaseboardRadiantConvectiveWater_DataFieldSet, ZoneHVACBaseboardRadiantConvectiveWater>
    {
        private IB_ZoneHVACBaseboardRadiantConvectiveWater_DataFieldSet() { }

    }
}
