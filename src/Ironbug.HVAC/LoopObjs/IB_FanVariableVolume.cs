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
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_FanVariableVolume();

        private static FanVariableVolume NewDefaultOpsObj(Model model) => new FanVariableVolume(model);
        public IB_FanVariableVolume() : base(NewDefaultOpsObj(new Model()))
        {
            
        }
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        
    }

    public sealed class IB_FanVariableVolume_DataFields 
        : IB_FieldSet<IB_FanVariableVolume_DataFields, FanVariableVolume>
    {
        
        private IB_FanVariableVolume_DataFields() {}

        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");

        public IB_Field FanEfficiency { get; }
            = new IB_BasicField("FanEfficiency", "Efficiency");

        public IB_Field PressureRise { get; }
            = new IB_BasicField("PressureRise", "PressureRise");

        public IB_Field MotorEfficiency { get; }
            = new IB_BasicField("MotorEfficiency", "MotorEfficiency");

        public IB_Field FanPowerCoefficient1 { get; }
            = new IB_BasicField("FanPowerCoefficient1", "Coefficient1");
        public IB_Field FanPowerCoefficient2 { get; }
            = new IB_BasicField("FanPowerCoefficient2", "Coefficient2");
        public  IB_Field FanPowerCoefficient3 { get; }
            = new IB_BasicField("FanPowerCoefficient3", "Coefficient3");
        public IB_Field FanPowerCoefficient4 { get; }
            = new IB_BasicField("FanPowerCoefficient4", "Coefficient4");
        public IB_Field FanPowerCoefficient5 { get; }
            = new IB_BasicField("FanPowerCoefficient5", "Coefficient5");
        


    }
}
