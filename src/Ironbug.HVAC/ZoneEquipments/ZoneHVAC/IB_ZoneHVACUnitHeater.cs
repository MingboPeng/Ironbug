using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACUnitHeater : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACUnitHeater();
        private static ZoneHVACUnitHeater NewDefaultOpsObj(Model model) 
            => new ZoneHVACUnitHeater(model,model.alwaysOnDiscreteSchedule(),new FanConstantVolume(model), new CoilHeatingElectric(model));

        private IB_CoilBasic HeatingCoil => this.GetChild<IB_CoilHeatingBasic>();
        private IB_Fan Fan => this.GetChild<IB_Fan>();

        [JsonConstructor]
        private IB_ZoneHVACUnitHeater(bool forDeserialization) : base(null)
        {
        }
        public IB_ZoneHVACUnitHeater(): base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(new IB_CoilHeatingElectric());
            this.AddChild(new IB_FanConstantVolume());
        }

        public void SetFan(IB_Fan Fan) => this.SetChild(Fan);

        public void SetHeatingCoil(IB_CoilHeatingBasic Coil)
        {
            this.SetChild(Coil);
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(InitMethodWithChildren, model);

            //Local Method
            ZoneHVACUnitHeater InitMethodWithChildren(Model md) =>
                new ZoneHVACUnitHeater(md, md.alwaysOnDiscreteSchedule(), this.Fan.ToOS(md), this.HeatingCoil.ToOS(md));
        }
        
    }
    public sealed class IB_ZoneHVACUnitHeater_FieldSet 
        : IB_FieldSet<IB_ZoneHVACUnitHeater_FieldSet, ZoneHVACUnitHeater>
    {
        private IB_ZoneHVACUnitHeater_FieldSet() {}

    }

}
