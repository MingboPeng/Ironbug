using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingDXTwoStageWithHumidityControlMode : IB_CoilDX
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingDXTwoStageWithHumidityControlMode();

        private static CoilCoolingDXTwoStageWithHumidityControlMode NewDefaultOpsObj(Model model) => new CoilCoolingDXTwoStageWithHumidityControlMode(model);

        public IB_CoilCoolingDXTwoStageWithHumidityControlMode() : base(NewDefaultOpsObj(new Model()))
        {
        }
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_CoilCoolingDXTwoStageWithHumidityControlMode_FieldSet
        : IB_FieldSet<IB_CoilCoolingDXTwoStageWithHumidityControlMode_FieldSet, CoilCoolingDXTwoStageWithHumidityControlMode>
    {
        private IB_CoilCoolingDXTwoStageWithHumidityControlMode_FieldSet() { }

    }
}
