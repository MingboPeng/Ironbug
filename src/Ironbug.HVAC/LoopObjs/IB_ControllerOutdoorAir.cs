using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ControllerOutdoorAir : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ControllerOutdoorAir();

        private static ControllerOutdoorAir InitMethod(Model model) => new ControllerOutdoorAir(model);
        public IB_ControllerOutdoorAir() : base(InitMethod(new Model()))
        {
        }

        public override IB_ModelObject Duplicate()
        {
            return this.DuplicateIBObj(() => new IB_ControllerOutdoorAir());
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            var newObj = (ControllerOutdoorAir)this.OnInitOpsObj(InitMethod, model); //TODO: would this work?
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
            = new IB_BasicDataField("Name", "Name");

        public IB_Field MinimumOutdoorAirFlowRate { get; }
            = new IB_BasicDataField("MinimumOutdoorAirFlowRate", "MinOAFlowRate");

        public IB_Field EconomizerControlType { get; }
            = new IB_BasicDataField("EconomizerControlType", "Economizer");


        public IB_Field HighHumidityControl { get; }
            = new IB_ProDataField("HighHumidityControl", "HiHumidityCtrl");

        
        
    }
}
