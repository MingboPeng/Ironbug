using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerFollowOutdoorAirTemperature : IB_SetpointManager
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerFollowOutdoorAirTemperature();

        private static SetpointManagerFollowOutdoorAirTemperature NewDefaultOpsObj(Model model) => new SetpointManagerFollowOutdoorAirTemperature(model);


        public IB_SetpointManagerFollowOutdoorAirTemperature() : base(NewDefaultOpsObj(new Model()))
        { 
        }
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_SetpointManagerFollowOutdoorAirTemperature_DataFieldSet
        : IB_FieldSet<IB_SetpointManagerFollowOutdoorAirTemperature_DataFieldSet, SetpointManagerFollowOutdoorAirTemperature>
    {
        private IB_SetpointManagerFollowOutdoorAirTemperature_DataFieldSet() { }

        public IB_Field ControlVariable { get; }
            = new IB_TopField("ControlVariable", "CtrlVar");

        public IB_Field ReferenceTemperatureType { get; }
            = new IB_TopField("ReferenceTemperatureType", "RefType");

        public IB_Field MaximumSetpointTemperature { get; }
            = new IB_TopField("MaximumSetpointTemperature", "MaxT");

        public IB_Field MinimumSetpointTemperature { get; }
           = new IB_TopField("MinimumSetpointTemperature", "MinT");

        public IB_Field OffsetTemperatureDifference { get; }
            = new IB_TopField("OffsetTemperatureDifference", "OffsetT");


    }
}
