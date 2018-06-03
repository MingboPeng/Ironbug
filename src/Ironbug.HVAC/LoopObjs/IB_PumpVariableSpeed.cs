using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_PumpVariableSpeed: IB_Pump
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_PumpVariableSpeed();

        private static PumpVariableSpeed InitMethod(Model model) => new PumpVariableSpeed(model);
        public IB_PumpVariableSpeed() : base(InitMethod(new Model()))
        {

        }
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((PumpVariableSpeed)this.ToOS(model)).addToNode(node);
        }
        

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model);
        }
    }

    public sealed class IB_PumpVariableSpeed_DataFields 
        : IB_FieldSet<IB_PumpVariableSpeed_DataFields, PumpVariableSpeed>
    {
        
        private IB_PumpVariableSpeed_DataFields(){ }

        public IB_Field RatedPumpHead { get; }
            = new IB_BasicDataField("RatedPumpHead", "PumpHead");
        
        public IB_Field MotorEfficiency { get; }
            = new IB_BasicDataField("MotorEfficiency", "Efficiency");

        public IB_Field RatedFlowRate { get; }
            = new IB_ProDataField("RatedFlowRate", "FlowRate");

        public IB_Field PumpControlType { get; }
            = new IB_ProDataField("PumpControlType", "ControlType");

        public IB_Field Coefficient1ofthePartLoadPerformanceCurve { get; }
            = new IB_ProDataField("Coefficient1ofthePartLoadPerformanceCurve", "Coefficient1");
        public IB_Field Coefficient2ofthePartLoadPerformanceCurve { get; }
            = new IB_ProDataField("Coefficient2ofthePartLoadPerformanceCurve", "Coefficient2");
        public IB_Field Coefficient3ofthePartLoadPerformanceCurve { get; }
            = new IB_ProDataField("Coefficient3ofthePartLoadPerformanceCurve", "Coefficient3");
        public IB_Field Coefficient4ofthePartLoadPerformanceCurve { get; }
            = new IB_ProDataField("Coefficient4ofthePartLoadPerformanceCurve", "Coefficient4");

        
    }
}
