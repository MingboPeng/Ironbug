using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctInletSideMixer : IB_AirTerminal
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctInletSideMixer(this.MixedZoneEquip);

        private static AirTerminalSingleDuctInletSideMixer NewDefaultOpsObj(Model model) => new AirTerminalSingleDuctInletSideMixer(model);

        public IB_ZoneEquipment MixedZoneEquip => this.Children.Get<IB_ZoneEquipment>();

        public IB_AirTerminalSingleDuctInletSideMixer(IB_ZoneEquipment ZoneEquipMixedWith) : base(NewDefaultOpsObj(new Model()))
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
