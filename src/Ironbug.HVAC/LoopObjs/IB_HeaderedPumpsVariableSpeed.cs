using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_HeaderedPumpsVariableSpeed : IB_Pump
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_HeaderedPumpsVariableSpeed();

        private static HeaderedPumpsVariableSpeed NewDefaultOpsObj(Model model) => new HeaderedPumpsVariableSpeed(model);
        public IB_HeaderedPumpsVariableSpeed():base(NewDefaultOpsObj)
        {
            
        }
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_HeaderedPumpsVariableSpeed_FieldSet
        : IB_FieldSet<IB_HeaderedPumpsVariableSpeed_FieldSet, HeaderedPumpsVariableSpeed>
    {
        private IB_HeaderedPumpsVariableSpeed_FieldSet() {}


        public IB_Field RatedPumpHead { get; }
            = new IB_BasicField("RatedPumpHead", "PumpHead");

        public IB_Field MotorEfficiency { get; }
            = new IB_BasicField("MotorEfficiency", "Efficiency");

        public IB_Field TotalRatedFlowRate { get; }
            = new IB_BasicField("TotalRatedFlowRate", "FlowRate");
        

    }
}
