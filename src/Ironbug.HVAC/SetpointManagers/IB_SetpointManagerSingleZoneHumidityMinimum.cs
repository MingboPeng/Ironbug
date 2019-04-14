using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerSingleZoneHumidityMinimum : IB_SetpointManager
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerSingleZoneHumidityMinimum(this.ControlZone);

        private static SetpointManagerSingleZoneHumidityMinimum NewDefaultOpsObj(Model model)
            => new SetpointManagerSingleZoneHumidityMinimum(model);

        private IB_ThermalZone ControlZone => this.Children.Get<IB_ThermalZone>();
        public IB_SetpointManagerSingleZoneHumidityMinimum(IB_ThermalZone thermalZone) : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(thermalZone);
            ((SetpointManagerSingleZoneHumidityMinimum)this.GhostOSObject).setControlZone((ThermalZone)thermalZone.ToOS(GhostOSObject.model()));
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
    public sealed class IB_SetpointManagerSingleZoneHumidityMinimum_FieldSet
        : IB_FieldSet<IB_SetpointManagerSingleZoneHumidityMinimum_FieldSet, SetpointManagerSingleZoneHumidityMinimum>
    {
        private IB_SetpointManagerSingleZoneHumidityMinimum_FieldSet() { }


    }
}
