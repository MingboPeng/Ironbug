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
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerOutdoorAirReset();

        private static SetpointManagerOutdoorAirReset NewDefaultOpsObj(Model model) => new SetpointManagerOutdoorAirReset(model);


        public IB_SetpointManagerOutdoorAirReset() : base(NewDefaultOpsObj(new Model()))
        { 
        }
        

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((SetpointManagerOutdoorAirReset)this.ToOS(model)).addToNode(node);
        }

        protected override ModelObject NewOpsObj(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model).to_SetpointManagerOutdoorAirReset().get();
        }
    }

    public sealed class IB_SetpointManagerOutdoorAirReset_DataFieldSet
        : IB_FieldSet<IB_SetpointManagerOutdoorAirReset_DataFieldSet, SetpointManagerOutdoorAirReset>
    {
        private IB_SetpointManagerOutdoorAirReset_DataFieldSet() { }

        public IB_Field SetpointatOutdoorHighTemperature { get; }
            = new IB_TopField("SetpointatOutdoorHighTemperature", "SpOHTemp");

        public IB_Field OutdoorHighTemperature { get; }
            = new IB_TopField("OutdoorHighTemperature", "OHTemp");

        public IB_Field SetpointatOutdoorLowTemperature { get; }
           = new IB_TopField("SetpointatOutdoorLowTemperature", "SpOLTemp");

        public IB_Field OutdoorLowTemperature { get; }
            = new IB_TopField("OutdoorLowTemperature", "OLTemp");


    }
}
