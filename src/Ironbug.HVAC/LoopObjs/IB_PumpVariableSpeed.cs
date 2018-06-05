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
            = new IB_BasicField("RatedPumpHead", "PumpHead");
        
        public IB_Field MotorEfficiency { get; }
            = new IB_BasicField("MotorEfficiency", "Efficiency");

        public IB_Field RatedFlowRate { get; }
            = new IB_ProField("RatedFlowRate", "FlowRate");

        public IB_Field PumpControlType { get; }
            = new IB_ProField("PumpControlType", "ControlType");

        public IB_Field Coefficient1ofthePartLoadPerformanceCurve { get; }
            = new IB_ProField("Coefficient1ofthePartLoadPerformanceCurve", "Coefficient1");
        public IB_Field Coefficient2ofthePartLoadPerformanceCurve { get; }
            = new IB_ProField("Coefficient2ofthePartLoadPerformanceCurve", "Coefficient2");
        public IB_Field Coefficient3ofthePartLoadPerformanceCurve { get; }
            = new IB_ProField("Coefficient3ofthePartLoadPerformanceCurve", "Coefficient3");
        public IB_Field Coefficient4ofthePartLoadPerformanceCurve { get; }
            = new IB_ProField("Coefficient4ofthePartLoadPerformanceCurve", "Coefficient4");

        
    }
}
