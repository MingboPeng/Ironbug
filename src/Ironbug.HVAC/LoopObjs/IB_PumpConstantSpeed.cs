using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_PumpConstantSpeed : IB_Pump
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_PumpConstantSpeed();

        private static PumpConstantSpeed InitMethod(Model model) => new PumpConstantSpeed(model);
        public IB_PumpConstantSpeed():base(InitMethod(new Model()))
        {
            
        }
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((PumpConstantSpeed)this.ToOS(model)).addToNode(node);
        }
        

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model);
        }
    }

    public sealed class IB_PumpConstantSpeed_DataFields 
        : IB_DataFieldSet<IB_PumpConstantSpeed_DataFields, PumpConstantSpeed>
    {
        private IB_PumpConstantSpeed_DataFields() {}


        public IB_DataField RatedPumpHead { get; }
            = new IB_BasicDataField("RatedPumpHead", "PumpHead");

        public IB_DataField MotorEfficiency { get; }
            = new IB_BasicDataField("MotorEfficiency", "Efficiency");

        public IB_DataField RatedFlowRate { get; }
            = new IB_ProDataField("RatedFlowRate", "FlowRate");

        public IB_DataField PumpControlType { get; }
            = new IB_ProDataField("PumpControlType", "ControlType");

       

    }
}
