using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_PumpVariableSpeed: IB_Pump
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_PumpVariableSpeed();

        private static PumpVariableSpeed NewDefaultOpsObj(Model model) => new PumpVariableSpeed(model);
        public IB_PumpVariableSpeed() : base(NewDefaultOpsObj(new Model()))
        {

        } 

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_PumpVariableSpeed_FieldSet 
        : IB_FieldSet<IB_PumpVariableSpeed_FieldSet, PumpVariableSpeed>
    {
        
        private IB_PumpVariableSpeed_FieldSet(){ }

        public IB_Field RatedPumpHead { get; }
            = new IB_BasicField("RatedPumpHead", "PumpHead");
        
        public IB_Field MotorEfficiency { get; }
            = new IB_BasicField("MotorEfficiency", "Efficiency");

        public IB_Field RatedFlowRate { get; }
            = new IB_BasicField("RatedFlowRate", "FlowRate");

        public IB_Field PumpControlType { get; }
            = new IB_BasicField("PumpControlType", "ControlType");

        public IB_Field Coefficient1ofthePartLoadPerformanceCurve { get; }
            = new IB_BasicField("Coefficient1ofthePartLoadPerformanceCurve", "Coefficient1");
        public IB_Field Coefficient2ofthePartLoadPerformanceCurve { get; }
            = new IB_BasicField("Coefficient2ofthePartLoadPerformanceCurve", "Coefficient2");
        public IB_Field Coefficient3ofthePartLoadPerformanceCurve { get; }
            = new IB_BasicField("Coefficient3ofthePartLoadPerformanceCurve", "Coefficient3");
        public IB_Field Coefficient4ofthePartLoadPerformanceCurve { get; }
            = new IB_BasicField("Coefficient4ofthePartLoadPerformanceCurve", "Coefficient4");

        
    }
}
