using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerSingleZoneReheat : IB_SetpointManager
    {
        
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerSingleZoneReheat(this.ControlZone.To<IB_ThermalZone>());

        private static SetpointManagerSingleZoneReheat InitMethod(Model model) 
            => new SetpointManagerSingleZoneReheat(model);

        private IB_Child ControlZone => this.Children.GetChild<IB_ThermalZone>();
        public IB_SetpointManagerSingleZoneReheat(IB_ThermalZone thermalZone) : base(InitMethod(new Model()))
        {
            var zone = new IB_Child(thermalZone, (obj) => this.SetControlZone(obj as IB_ThermalZone));
            this.Children.Add(zone);

            ((SetpointManagerSingleZoneReheat)this.GhostOSObject).setControlZone((ThermalZone)thermalZone.ToOS(GhostOSObject.model()));
        }

        public void SetControlZone(IB_ThermalZone zone)
        {
            this.ControlZone.Set(zone);
        }
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((SetpointManagerSingleZoneReheat)this.ToOS(model)).addToNode(node);
        }
        
        protected override ModelObject InitOpsObj(Model model)
        {
            var newObj = base.OnInitOpsObj(InitMethod, model).to_SetpointManagerSingleZoneReheat().get();
            var zone = (ThermalZone)this.ControlZone.To<IB_ThermalZone>().ToOS(model);
            newObj.setControlZone(zone);
            return newObj;
        }
    }
    public sealed class IB_SetpointManagerSingleZoneReheat_DataFieldSet
        : IB_FieldSet<IB_SetpointManagerSingleZoneReheat_DataFieldSet, SetpointManagerSingleZoneReheat>
    {
        private IB_SetpointManagerSingleZoneReheat_DataFieldSet() { }

        public IB_Field MaximumSupplyAirTemperature { get; }
            = new IB_TopField("MaximumSupplyAirTemperature", "maxTemp");

        public IB_Field MinimumSupplyAirTemperature { get; }
            = new IB_TopField("MinimumSupplyAirTemperature", "minTemp");
    }
}
