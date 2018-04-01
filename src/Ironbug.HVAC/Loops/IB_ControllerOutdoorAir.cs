using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ControllerOutdoorAir : IB_ModelObject
    {
        private static ControllerOutdoorAir InitMethod(Model model) => new ControllerOutdoorAir(model);
        public IB_ControllerOutdoorAir() : base(InitMethod(new Model()))
        {
        }
        
        public override IB_ModelObject Duplicate()
        {
            return this.DuplicateIBObj(() => new IB_ControllerOutdoorAir());
        }

        public ModelObject ToOS(Model model)
        {
            return (ControllerOutdoorAir)this.ToOS(InitMethod, model);
        }
    }

    public class IB_ControllerOutdoorAir_DataFieldSet 
        : IB_DataFieldSet<IB_ControllerOutdoorAir_DataFieldSet, ControllerOutdoorAir>
    {
        private IB_ControllerOutdoorAir_DataFieldSet() {}
        
        public IB_DataField Name { get; }
            = new IB_BasicDataField("Name", "Name");

        public IB_DataField MinimumOutdoorAirFlowRate { get; }
            = new IB_BasicDataField("MinimumOutdoorAirFlowRate", "MinOAFlowRate");

        public IB_DataField EconomizerControlType { get; }
            = new IB_BasicDataField("EconomizerControlType", "Economizer");


        public IB_DataField HighHumidityControl { get; }
            = new IB_ProDataField("HighHumidityControl", "HiHumidityCtrl");

        
        
    }
}
