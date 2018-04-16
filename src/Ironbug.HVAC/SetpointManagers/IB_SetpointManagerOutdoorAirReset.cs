using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerOutdoorAirReset : IB_SetpointManager
    {
        private static SetpointManagerOutdoorAirReset InitMethod(Model model) => new SetpointManagerOutdoorAirReset(model);


        public IB_SetpointManagerOutdoorAirReset() : base(InitMethod(new Model()))
        { 
        }


        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_SetpointManagerOutdoorAirReset());
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((SetpointManagerOutdoorAirReset)this.ToOS(model)).addToNode(node);
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_SetpointManagerOutdoorAirReset().get();
        }
    }

    public sealed class IB_SetpointManagerOutdoorAirReset_DataFieldSet
        : IB_DataFieldSet<IB_SetpointManagerOutdoorAirReset_DataFieldSet, SetpointManagerOutdoorAirReset>
    {
        private IB_SetpointManagerOutdoorAirReset_DataFieldSet() { }

        public IB_DataField SetpointatOutdoorHighTemperature { get; }
            = new IB_MandatoryDataField("SetpointatOutdoorHighTemperature", "SpOHTemp");

        public IB_DataField OutdoorHighTemperature { get; }
            = new IB_MandatoryDataField("OutdoorHighTemperature", "OHTemp");

        public IB_DataField SetpointatOutdoorLowTemperature { get; }
           = new IB_MandatoryDataField("SetpointatOutdoorLowTemperature", "SpOLTemp");

        public IB_DataField OutdoorLowTemperature { get; }
            = new IB_MandatoryDataField("OutdoorLowTemperature", "OLTemp");


    }
}
