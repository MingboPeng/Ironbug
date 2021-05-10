using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerSingleZoneCooling : IB_SetpointManager
    {
        
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerSingleZoneCooling(this.ControlZone);

        private static SetpointManagerSingleZoneCooling NewDefaultOpsObj(Model model) 
            => new SetpointManagerSingleZoneCooling(model);

        private IB_ThermalZone ControlZone => this.GetChild<IB_ThermalZone>();
        public IB_SetpointManagerSingleZoneCooling(IB_ThermalZone thermalZone) : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(thermalZone);
            ((SetpointManagerSingleZoneCooling)this.GhostOSObject).setControlZone((ThermalZone)thermalZone.ToOS(GhostOSObject.model()));
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
    public sealed class IB_SetpointManagerSingleZoneCooling_FieldSet
        : IB_FieldSet<IB_SetpointManagerSingleZoneCooling_FieldSet, SetpointManagerSingleZoneCooling>
    {
        private IB_SetpointManagerSingleZoneCooling_FieldSet() { }

    }
}
