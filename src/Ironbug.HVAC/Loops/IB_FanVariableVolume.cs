using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ironbug.HVAC.BaseClass;
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
            return base.DuplicateIBObj(() => new IB_FanVariableVolume());
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

    public class IB_FanVariableVolume_DataFields 
        : IB_DataFieldSet<IB_FanVariableVolume_DataFields, FanVariableVolume>
    {
        
        private IB_FanVariableVolume_DataFields() {}

        public IB_DataField Name { get; }
            = new IB_BasicDataField("Name", "Name");

        public IB_DataField FanEfficiency { get; }
            = new IB_BasicDataField("FanEfficiency", "Efficiency");

        public IB_DataField PressureRise { get; }
            = new IB_BasicDataField("PressureRise", "PressureRise");

        public IB_DataField MotorEfficiency { get; }
            = new IB_ProDataField("MotorEfficiency", "MotorEfficiency");

        public IB_DataField FanPowerCoefficient1 { get; }
            = new IB_ProDataField("FanPowerCoefficient1", "Coefficient1");
        public IB_DataField FanPowerCoefficient2 { get; }
            = new IB_ProDataField("FanPowerCoefficient2", "Coefficient2");
        public  IB_DataField FanPowerCoefficient3 { get; }
            = new IB_ProDataField("FanPowerCoefficient3", "Coefficient3");
        public IB_DataField FanPowerCoefficient4 { get; }
            = new IB_ProDataField("FanPowerCoefficient4", "Coefficient4");
        public IB_DataField FanPowerCoefficient5 { get; }
            = new IB_ProDataField("FanPowerCoefficient5", "Coefficient5");
        


    }
}
