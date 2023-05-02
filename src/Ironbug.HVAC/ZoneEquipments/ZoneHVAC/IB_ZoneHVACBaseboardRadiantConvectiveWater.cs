using System;
using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACBaseboardRadiantConvectiveWater : BaseClass.IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACBaseboardRadiantConvectiveWater();

        private static ZoneHVACBaseboardRadiantConvectiveWater NewDefaultOpsObj(Model model) 
            => new ZoneHVACBaseboardRadiantConvectiveWater(model);

        private IB_CoilHeatingWaterBaseboardRadiant HeatingCoil => this.GetChild<IB_CoilHeatingWaterBaseboardRadiant>();

        [JsonConstructor]
        private IB_ZoneHVACBaseboardRadiantConvectiveWater(bool forDeserialization): base(null)
        {
        }

        public IB_ZoneHVACBaseboardRadiantConvectiveWater() : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(new IB_CoilHeatingWaterBaseboardRadiant());
        }

        public void SetHeatingCoil(IB_CoilHeatingWaterBaseboardRadiant Coil)
        {
            this.SetChild(Coil);
        }

        public override HVACComponent ToOS(Model model)
        {
            var opsObj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            if (this.HeatingCoil != null) opsObj.setHeatingCoil(this.HeatingCoil.ToOS(model));
            return opsObj;
        }

    }

    public sealed class IB_ZoneHVACBaseboardRadiantConvectiveWater_FieldSet
        : IB_FieldSet<IB_ZoneHVACBaseboardRadiantConvectiveWater_FieldSet, ZoneHVACBaseboardRadiantConvectiveWater>
    {
        private IB_ZoneHVACBaseboardRadiantConvectiveWater_FieldSet() { }

    }
}
