using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_FanConstantVolume : IB_Fan
    {
        //Fan Constant Volume 1 !- Name  
        //0.7                  !- Fan Efficiency(Default: 0.7)
        //250.0                !- Pressure Rise[kg / m * s ^ 2]  (Default: 250.0)
        //AutoSize             !- Maximum Flow Rate[m ^ 3 / s] 
        //0.9                  !- Motor Efficiency(Default: 0.9)
        //1.0                  !- Motor In Airstream Fraction(Default: 1.0)
        //General              !- End-Use Subcategory(Default: General)

        //private FanConstantVolume osFanConstantVolume { get; set; }
        private static FanConstantVolume InitMethod(Model model) => new FanConstantVolume(model);

        public IB_FanConstantVolume():base(InitMethod(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((FanConstantVolume)this.ToOS(model)).addToNode(node);
        }

        public override ModelObject ToOS(Model model)
        {
            return (FanConstantVolume)base.ToOS(InitMethod, model);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_FanConstantVolume());
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
