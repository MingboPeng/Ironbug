using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_HeaderedPumpsConstantSpeed : IB_Pump
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_HeaderedPumpsConstantSpeed();

        private static HeaderedPumpsConstantSpeed NewDefaultOpsObj(Model model) => new HeaderedPumpsConstantSpeed(model);
        public IB_HeaderedPumpsConstantSpeed():base(NewDefaultOpsObj(new Model()))
        {
            
        }
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_HeaderedPumpsConstantSpeed_FieldSet
        : IB_FieldSet<IB_HeaderedPumpsConstantSpeed_FieldSet, HeaderedPumpsConstantSpeed>
    {
        private IB_HeaderedPumpsConstantSpeed_FieldSet() {}


        public IB_Field RatedPumpHead { get; }
            = new IB_BasicField("RatedPumpHead", "PumpHead");

        public IB_Field MotorEfficiency { get; }
            = new IB_BasicField("MotorEfficiency", "Efficiency");

        public IB_Field TotalRatedFlowRate { get; }
            = new IB_BasicField("TotalRatedFlowRate", "FlowRate");
        

    }
}
