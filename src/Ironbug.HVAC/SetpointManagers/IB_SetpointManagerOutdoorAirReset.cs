using System;
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
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_SetpointManagerOutdoorAirReset_FieldSet
        : IB_FieldSet<IB_SetpointManagerOutdoorAirReset_FieldSet, SetpointManagerOutdoorAirReset>
    {
        private IB_SetpointManagerOutdoorAirReset_FieldSet() { }

        public IB_Field SetpointatOutdoorHighTemperature { get; }
            = new IB_BasicField("SetpointatOutdoorHighTemperature", "SpOHTemp");

        public IB_Field OutdoorHighTemperature { get; }
            = new IB_BasicField("OutdoorHighTemperature", "OHTemp");

        public IB_Field SetpointatOutdoorLowTemperature { get; }
           = new IB_BasicField("SetpointatOutdoorLowTemperature", "SpOLTemp");

        public IB_Field OutdoorLowTemperature { get; }
            = new IB_BasicField("OutdoorLowTemperature", "OLTemp");


    }
}
