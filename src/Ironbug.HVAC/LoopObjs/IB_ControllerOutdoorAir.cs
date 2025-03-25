using System;
using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ControllerOutdoorAir : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ControllerOutdoorAir();

        private static ControllerOutdoorAir NewDefaultOpsObj(Model model) => new ControllerOutdoorAir(model);
        private IB_ControllerMechanicalVentilation ControllerMechanicalVentilation => this.GetChild<IB_ControllerMechanicalVentilation>();

        [JsonConstructor]
        private IB_ControllerOutdoorAir(bool forDerialization) : base(NewDefaultOpsObj)
        {
        }

        public IB_ControllerOutdoorAir() : base(NewDefaultOpsObj)
        {
            this.AddChild(new IB_ControllerMechanicalVentilation());
        }

        public void SetMechanicalVentilation(IB_ControllerMechanicalVentilation mechanicalVentilation)
        {
            this.SetChild(mechanicalVentilation);
        }
        
        public ModelObject ToOS(Model model)
        {
            var newObj = this.OnNewOpsObj(NewDefaultOpsObj, model);
            var newMechVent = (ControllerMechanicalVentilation)this.ControllerMechanicalVentilation.ToOS(model);
            newObj.setControllerMechanicalVentilation(newMechVent);

            return newObj;
        }

    }

    public sealed class IB_ControllerOutdoorAir_FieldSet 
        : IB_FieldSet<IB_ControllerOutdoorAir_FieldSet, ControllerOutdoorAir>
    {
        private IB_ControllerOutdoorAir_FieldSet() {}
        
        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");

        public IB_Field MinimumOutdoorAirFlowRate { get; }
            = new IB_BasicField("MinimumOutdoorAirFlowRate", "MinOAFlowRate");

        public IB_Field EconomizerControlType { get; }
            = new IB_BasicField("EconomizerControlType", "Economizer");


        public IB_Field HighHumidityControl { get; }
            = new IB_BasicField("HighHumidityControl", "HiHumidityCtrl");

        
        
    }
}
