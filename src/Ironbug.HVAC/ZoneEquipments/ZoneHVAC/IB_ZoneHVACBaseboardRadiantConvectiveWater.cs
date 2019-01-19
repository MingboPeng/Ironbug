using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACBaseboardRadiantConvectiveWater : BaseClass.IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACBaseboardRadiantConvectiveWater();

        private static ZoneHVACBaseboardRadiantConvectiveWater NewDefaultOpsObj(Model model) 
            => new ZoneHVACBaseboardRadiantConvectiveWater(model);

        private IB_CoilHeatingWaterBaseboardRadiant HeatingCoil => this.Children.Get<IB_CoilHeatingWaterBaseboardRadiant>();

        public IB_ZoneHVACBaseboardRadiantConvectiveWater() : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(new IB_CoilHeatingWaterBaseboardRadiant());
        }

        public void SetHeatingCoil(IB_CoilHeatingWaterBaseboardRadiant Coil)
        {
            this.SetChild(Coil);
        }

        protected override ModelObject NewOpsObj(Model model)
        {
            var opsObj =  base.OnNewOpsObj(NewDefaultOpsObj, model).to_ZoneHVACBaseboardRadiantConvectiveWater().get();
            opsObj.setHeatingCoil(this.HeatingCoil.ToOS(model));
            return opsObj;
        }
    }

    public sealed class IB_ZoneHVACBaseboardRadiantConvectiveWater_DataFieldSet
        : IB_FieldSet<IB_ZoneHVACBaseboardRadiantConvectiveWater_DataFieldSet, ZoneHVACBaseboardRadiantConvectiveWater>
    {
        private IB_ZoneHVACBaseboardRadiantConvectiveWater_DataFieldSet() { }

    }
}
