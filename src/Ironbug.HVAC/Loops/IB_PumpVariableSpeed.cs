using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_PumpVariableSpeed: IB_Pump
    {
        private static PumpVariableSpeed InitMethod(Model model) => new PumpVariableSpeed(model);
        public IB_PumpVariableSpeed() : base(InitMethod(new Model()))
        {

        }
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((PumpVariableSpeed)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_PumpVariableSpeed());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model);
        }
    }

    public sealed class IB_PumpVariableSpeed_DataFields 
        : IB_DataFieldSet<IB_PumpVariableSpeed_DataFields, PumpVariableSpeed>
    {
        
        private IB_PumpVariableSpeed_DataFields(){ }

        public IB_DataField RatedPumpHead { get; }
            = new IB_BasicDataField("RatedPumpHead", "PumpHead");
        
        public IB_DataField MotorEfficiency { get; }
            = new IB_BasicDataField("MotorEfficiency", "Efficiency");

        public IB_DataField RatedFlowRate { get; }
            = new IB_ProDataField("RatedFlowRate", "FlowRate");

        public IB_DataField PumpControlType { get; }
            = new IB_ProDataField("PumpControlType", "ControlType");

        public IB_DataField Coefficient1ofthePartLoadPerformanceCurve { get; }
            = new IB_ProDataField("Coefficient1ofthePartLoadPerformanceCurve", "Coefficient1");
        public IB_DataField Coefficient2ofthePartLoadPerformanceCurve { get; }
            = new IB_ProDataField("Coefficient2ofthePartLoadPerformanceCurve", "Coefficient2");
        public IB_DataField Coefficient3ofthePartLoadPerformanceCurve { get; }
            = new IB_ProDataField("Coefficient3ofthePartLoadPerformanceCurve", "Coefficient3");
        public IB_DataField Coefficient4ofthePartLoadPerformanceCurve { get; }
            = new IB_ProDataField("Coefficient4ofthePartLoadPerformanceCurve", "Coefficient4");

        
    }
}
