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
            return base.DuplicateIB_ModelObject(() => new IB_PumpVariableSpeed());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model);
        }
    }

    public class IB_PumpVariableSpeed_DataFields : IB_DataFieldSet
    {
        protected override IddObject RefIddObject => new PumpVariableSpeed(new Model()).iddObject();

        protected override Type ParentType => typeof(PumpVariableSpeed);

        public static readonly IB_DataField RatedPumpHead
            = new IB_DataField("RatedPumpHead", "PumpHead", dbType, true);

        public static readonly IB_DataField MotorEfficiency
            = new IB_DataField("MotorEfficiency", "Efficiency", dbType, true);

        public static readonly IB_DataField RatedFlowRate
            = new IB_DataField("RatedFlowRate", "FlowRate", dbType);

        public static readonly IB_DataField PumpControlType
            = new IB_DataField("PumpControlType", "ControlType", dbType);

        public static readonly IB_DataField Coefficient1ofthePartLoadPerformanceCurve
            = new IB_DataField("Coefficient1ofthePartLoadPerformanceCurve", "Coefficient1", dbType);
        public static readonly IB_DataField Coefficient2ofthePartLoadPerformanceCurve
            = new IB_DataField("Coefficient2ofthePartLoadPerformanceCurve", "Coefficient2", dbType);
        public static readonly IB_DataField Coefficient3ofthePartLoadPerformanceCurve
            = new IB_DataField("Coefficient3ofthePartLoadPerformanceCurve", "Coefficient3", dbType);
        public static readonly IB_DataField Coefficient4ofthePartLoadPerformanceCurve
            = new IB_DataField("Coefficient4ofthePartLoadPerformanceCurve", "Coefficient4", dbType);

        
    }
}
