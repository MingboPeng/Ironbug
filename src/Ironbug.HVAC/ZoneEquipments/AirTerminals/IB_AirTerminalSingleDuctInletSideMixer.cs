using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctInletSideMixer : IB_AirTerminal
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctInletSideMixer(this.MixedZoneEquip);

        private static AirTerminalSingleDuctInletSideMixer NewDefaultOpsObj(Model model) => new AirTerminalSingleDuctInletSideMixer(model);

        public IB_ZoneEquipment MixedZoneEquip => this.GetChild<IB_ZoneEquipment>();
        [JsonConstructor]
        private IB_AirTerminalSingleDuctInletSideMixer() : base(null) { }
        public IB_AirTerminalSingleDuctInletSideMixer(IB_ZoneEquipment ZoneEquipMixedWith) : base(NewDefaultOpsObj)
        {
            this.AddChild(ZoneEquipMixedWith);
        }

        public override HVACComponent ToOS(Model model)
        {

            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }
    public sealed class IB_AirTerminalSingleDuctInletSideMixer_FieldSet
    : IB_FieldSet<IB_AirTerminalSingleDuctInletSideMixer_FieldSet, AirTerminalSingleDuctInletSideMixer>
    {
        private IB_AirTerminalSingleDuctInletSideMixer_FieldSet() { }

    }
}
