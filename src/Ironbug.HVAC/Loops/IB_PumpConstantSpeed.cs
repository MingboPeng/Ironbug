using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_PumpConstantSpeed : IB_Pump
    {
        private static PumpConstantSpeed InitMethod(Model model) => new PumpConstantSpeed(model);
        public IB_PumpConstantSpeed():base(InitMethod(new Model()))
        {

        }
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((PumpConstantSpeed)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIB_ModelObject(() => new IB_PumpConstantSpeed());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model);
        }
    }

    public class IB_PumpConstantSpeed_DataFields : IB_DataFieldSet
    {
        protected override IddObject RefIddObject => new PumpConstantSpeed(new Model()).iddObject();

        public static readonly IB_DataField RatedPumpHead
            = new IB_DataField("RatedPumpHead", "PumpHead", dbType, true);

        public static readonly IB_DataField MotorEfficiency
            = new IB_DataField("MotorEfficiency", "Efficiency", dbType, true);

        public static readonly IB_DataField RatedFlowRate
            = new IB_DataField("RatedFlowRate", "FlowRate", dbType);

        public static readonly IB_DataField PumpControlType
            = new IB_DataField("PumpControlType", "ControlType", dbType);



    }
}
