using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerSingleZoneReheat : IB_SetpointManager
    {
        
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerSingleZoneReheat(this.ControlZone);

        private static SetpointManagerSingleZoneReheat NewDefaultOpsObj(Model model) 
            => new SetpointManagerSingleZoneReheat(model);

        private IB_ThermalZone ControlZone => this.GetChild<IB_ThermalZone>();
        private IB_SetpointManagerSingleZoneReheat() : base(null) { }
        public IB_SetpointManagerSingleZoneReheat(IB_ThermalZone thermalZone) : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(thermalZone);

            ((SetpointManagerSingleZoneReheat)this.GhostOSObject).setControlZone((ThermalZone)thermalZone.ToOS(GhostOSObject.model()));
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
    public sealed class IB_SetpointManagerSingleZoneReheat_FieldSet
        : IB_FieldSet<IB_SetpointManagerSingleZoneReheat_FieldSet, SetpointManagerSingleZoneReheat>
    {
        private IB_SetpointManagerSingleZoneReheat_FieldSet() { }

        public IB_Field MaximumSupplyAirTemperature { get; }
            = new IB_TopField("MaximumSupplyAirTemperature", "maxTemp");

        public IB_Field MinimumSupplyAirTemperature { get; }
            = new IB_TopField("MinimumSupplyAirTemperature", "minTemp");
    }
}
