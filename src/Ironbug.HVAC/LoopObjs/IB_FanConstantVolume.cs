using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_FanConstantVolume : IB_Fan
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_FanConstantVolume();

        private static FanConstantVolume InitMethod(Model model) => new FanConstantVolume(model);

        public IB_FanConstantVolume():base(InitMethod(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((FanConstantVolume)this.ToOS(model)).addToNode(node);
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            return (FanConstantVolume)base.OnInitOpsObj(InitMethod, model);
        }
        
    }



    public sealed class IB_FanConstantVolume_DataFields 
        : IB_DataFieldSet<IB_FanConstantVolume_DataFields, FanConstantVolume>
    {
        private IB_FanConstantVolume_DataFields() {}

        public IB_DataField Name { get; }
            = new IB_BasicDataField("Name", "Name");

        public IB_DataField FanEfficiency { get; }
            = new IB_BasicDataField("FanEfficiency", "Efficiency");

        public IB_DataField PressureRise { get; }
            = new IB_ProDataField("PressureRise", "PressureRise");

        public IB_DataField MotorEfficiency { get; }
            = new IB_ProDataField("MotorEfficiency", "MotorEfficiency");

        
    }
}
