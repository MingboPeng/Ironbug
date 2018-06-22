using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ControllerOutdoorAir : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ControllerOutdoorAir();

        private static ControllerOutdoorAir InitMethod(Model model) => new ControllerOutdoorAir(model);
        private IB_Child ControllerMechanicalVentilation => this.Children.GetChild<IB_ControllerMechanicalVentilation>();
        public IB_ControllerOutdoorAir() : base(InitMethod(new Model()))
        {
        }

        public void SetMechanicalVentilation(IB_ControllerMechanicalVentilation mechanicalVentilation)
        {
            this.ControllerMechanicalVentilation.Set(mechanicalVentilation);
        }

        public override IB_ModelObject Duplicate()
        {
            var newObj = this.DuplicateIBObj(() => new IB_ControllerOutdoorAir());
            var newMechVent = (IB_ControllerMechanicalVentilation)this.ControllerMechanicalVentilation.DuplicateChild();
            newObj.SetMechanicalVentilation(newMechVent);
            return newObj;
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            var newObj = (ControllerOutdoorAir)this.OnInitOpsObj(InitMethod, model); //TODO: would this work?
            var newMechVent = (ControllerMechanicalVentilation)this.ControllerMechanicalVentilation.To<IB_ControllerMechanicalVentilation>().ToOS(model);
            newObj.setControllerMechanicalVentilation(newMechVent);

            return newObj;
        }

        public ModelObject ToOS(Model model)
        {
            return this.InitOpsObj(model);
        }

    }

    public sealed class IB_ControllerOutdoorAir_DataFieldSet 
        : IB_FieldSet<IB_ControllerOutdoorAir_DataFieldSet, ControllerOutdoorAir>
    {
        private IB_ControllerOutdoorAir_DataFieldSet() {}
        
        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");

        public IB_Field MinimumOutdoorAirFlowRate { get; }
            = new IB_BasicField("MinimumOutdoorAirFlowRate", "MinOAFlowRate");

        public IB_Field EconomizerControlType { get; }
            = new IB_BasicField("EconomizerControlType", "Economizer");


        public IB_Field HighHumidityControl { get; }
            = new IB_ProField("HighHumidityControl", "HiHumidityCtrl");

        
        
    }
}
