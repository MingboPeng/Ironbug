using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_FanVariableVolume : IB_Fan
    {
        private static FanVariableVolume InitMethod(Model model) => new FanVariableVolume(model);
        public IB_FanVariableVolume() : base(InitMethod(new Model()))
        {
            
        }   
        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIB_ModelObject(() => new IB_FanVariableVolume());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model);
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((FanVariableVolume)this.ToOS(model)).addToNode(node);
        }
    }

    public class IB_FanVariableVolume_DataFields : IB_DataFieldSet
    {
        protected override IddObject RefIddObject => new FanVariableVolume(new Model()).iddObject();
        //https://openstudio-sdk-documentation.s3.amazonaws.com/cpp/OpenStudio-2.4.0-doc/model/html/classopenstudio_1_1model_1_1_fan_constant_volume.html

        protected override Type ParentType => typeof(FanVariableVolume);

        public static readonly IB_DataField Name
            = new IB_DataField("Name", "Name", strType, true);

        public static readonly IB_DataField FanEfficiency
            = new IB_DataField("FanEfficiency", "Efficiency", dbType, true);

        public static readonly IB_DataField PressureRise
            = new IB_DataField("PressureRise", "PressureRise", dbType, true);

        public static readonly IB_DataField MotorEfficiency
            = new IB_DataField("MotorEfficiency", "MotorEfficiency", dbType);

        public static readonly IB_DataField FanPowerCoefficient1
            = new IB_DataField("FanPowerCoefficient1", "Coefficient1", dbType);
        public static readonly IB_DataField FanPowerCoefficient2
            = new IB_DataField("FanPowerCoefficient2", "Coefficient2", dbType);
        public static readonly IB_DataField FanPowerCoefficient3
            = new IB_DataField("FanPowerCoefficient3", "Coefficient3", dbType);
        public static readonly IB_DataField FanPowerCoefficient4
            = new IB_DataField("FanPowerCoefficient4", "Coefficient4", dbType);
        public static readonly IB_DataField FanPowerCoefficient5
            = new IB_DataField("FanPowerCoefficient5", "Coefficient5", dbType);
        


    }
}
