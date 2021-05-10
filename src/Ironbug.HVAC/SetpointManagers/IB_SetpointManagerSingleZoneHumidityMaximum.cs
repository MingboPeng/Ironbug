using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerSingleZoneHumidityMaximum : IB_SetpointManager
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerSingleZoneHumidityMaximum(this.ControlZone);

        private static SetpointManagerSingleZoneHumidityMaximum NewDefaultOpsObj(Model model)
            => new SetpointManagerSingleZoneHumidityMaximum(model);

        private IB_ThermalZone ControlZone => this.GetChild<IB_ThermalZone>();
        public IB_SetpointManagerSingleZoneHumidityMaximum(IB_ThermalZone thermalZone) : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(thermalZone);
            ((SetpointManagerSingleZoneHumidityMaximum)this.GhostOSObject).setControlZone((ThermalZone)thermalZone.ToOS(GhostOSObject.model()));
        }

        public void SetControlZone(IB_ThermalZone zone)
        {
            this.SetChild(zone);
        }

        public override HVACComponent ToOS(Model model)
        {
            var newObj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            var zone = (ThermalZone)this.ControlZone.ToOS(model);
            newObj.setControlZone(zone);
            return newObj;
        }
    }
    public sealed class IB_SetpointManagerSingleZoneHumidityMaximum_FieldSet
        : IB_FieldSet<IB_SetpointManagerSingleZoneHumidityMaximum_FieldSet, SetpointManagerSingleZoneHumidityMaximum>
    {
        private IB_SetpointManagerSingleZoneHumidityMaximum_FieldSet() { }


    }
}
