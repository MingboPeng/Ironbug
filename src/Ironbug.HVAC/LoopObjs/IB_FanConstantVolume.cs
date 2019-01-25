using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_FanConstantVolume : IB_Fan
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_FanConstantVolume();

        private static FanConstantVolume NewDefaultOpsObj(Model model) => new FanConstantVolume(model);

        public IB_FanConstantVolume():base(NewDefaultOpsObj(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((FanConstantVolume)this.ToOS(model)).addToNode(node);
        }

        protected override ModelObject NewOpsObj(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        
    }



    public sealed class IB_FanConstantVolume_DataFields 
        : IB_FieldSet<IB_FanConstantVolume_DataFields, FanConstantVolume>
    {
        private IB_FanConstantVolume_DataFields() {}

        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");

        public IB_Field FanEfficiency { get; }
            = new IB_BasicField("FanEfficiency", "Efficiency");

        public IB_Field PressureRise { get; }
            = new IB_BasicField("PressureRise", "PressureRise");

        public IB_Field MotorEfficiency { get; }
            = new IB_BasicField("MotorEfficiency", "MotorEfficiency");

        
    }
}
