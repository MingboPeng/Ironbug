using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerFollowOutdoorAirTemperature : IB_SetpointManager
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerFollowOutdoorAirTemperature();

        private static SetpointManagerFollowOutdoorAirTemperature InitMethod(Model model) => new SetpointManagerFollowOutdoorAirTemperature(model);


        public IB_SetpointManagerFollowOutdoorAirTemperature() : base(InitMethod(new Model()))
        { 
        }
        

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((SetpointManagerFollowOutdoorAirTemperature)this.ToOS(model)).addToNode(node);
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_SetpointManagerFollowOutdoorAirTemperature().get();
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
