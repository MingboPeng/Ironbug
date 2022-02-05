using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerSingleZoneHeating : IB_SetpointManager
    {
        
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerSingleZoneHeating(this.ControlZone);

        private static SetpointManagerSingleZoneHeating NewDefaultOpsObj(Model model) 
            => new SetpointManagerSingleZoneHeating(model);

        private IB_ThermalZone ControlZone => this.GetChild<IB_ThermalZone>();
        private IB_SetpointManagerSingleZoneHeating() : base(null) { }
        public IB_SetpointManagerSingleZoneHeating(IB_ThermalZone thermalZone) : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(thermalZone);
            ((SetpointManagerSingleZoneHeating)this.GhostOSObject).setControlZone((ThermalZone)thermalZone.ToOS(GhostOSObject.model()));
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
    public sealed class IB_SetpointManagerSingleZoneHeating_FieldSet
        : IB_FieldSet<IB_SetpointManagerSingleZoneHeating_FieldSet, SetpointManagerSingleZoneHeating>
    {
        private IB_SetpointManagerSingleZoneHeating_FieldSet() { }

    }
}
