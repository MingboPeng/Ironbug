using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{


    public class IB_CoilCoolingDXTwoStageWithHumidityControlMode : IB_CoilDX
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingDXTwoStageWithHumidityControlMode();

        private static CoilCoolingDXTwoStageWithHumidityControlMode NewDefaultOpsObj(Model model) => new CoilCoolingDXTwoStageWithHumidityControlMode(model);

        private IB_CoilPerformanceDXCooling _normalStage1 => this.GetChild<IB_CoilPerformanceDXCooling>(0);
        private IB_CoilPerformanceDXCooling _normalStage1p2 => this.GetChild<IB_CoilPerformanceDXCooling>(1);
        private IB_CoilPerformanceDXCooling _dehumidificationStage1 => this.GetChild<IB_CoilPerformanceDXCooling>(2);
        private IB_CoilPerformanceDXCooling _dehumidificationStage1p2 => this.GetChild<IB_CoilPerformanceDXCooling>(3);

        public IB_CoilCoolingDXTwoStageWithHumidityControlMode() : base(NewDefaultOpsObj(new Model()))
        {
            // add placeholders
            this.AddChild(null);
            this.AddChild(null);
            this.AddChild(null);
            this.AddChild(null);
        }
        
        public void SetNormalModeStage1CoilPerformance(IB_CoilPerformanceDXCooling coil)
        {
            this.SetChild(0, coil);
        }
        public void SetNormalModeStage1Plus2CoilPerformance(IB_CoilPerformanceDXCooling coil)
        {
            this.SetChild(1, coil);
        }
        public void SetDehumidificationMode1Stage1CoilPerformance(IB_CoilPerformanceDXCooling coil)
        {
            this.SetChild(2, coil);
        }
        public void SetDehumidificationMode1Stage1Plus2CoilPerformance(IB_CoilPerformanceDXCooling coil)
        {
            this.SetChild(3, coil);
        }

        public override HVACComponent ToOS(Model model)
        {
            var opsObj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            if(_normalStage1 != null) opsObj.setNormalModeStage1CoilPerformance(_normalStage1.ToOS(model));
            if (_normalStage1p2 != null) opsObj.setNormalModeStage1Plus2CoilPerformance(_normalStage1p2.ToOS(model));
            if (_dehumidificationStage1 != null) opsObj.setDehumidificationMode1Stage1CoilPerformance(_dehumidificationStage1.ToOS(model));
            if (_dehumidificationStage1p2 != null) opsObj.setDehumidificationMode1Stage1Plus2CoilPerformance(_dehumidificationStage1p2.ToOS(model));
           
            return opsObj;
        }
    }

    public sealed class IB_CoilCoolingDXTwoStageWithHumidityControlMode_FieldSet
        : IB_FieldSet<IB_CoilCoolingDXTwoStageWithHumidityControlMode_FieldSet, CoilCoolingDXTwoStageWithHumidityControlMode>
    {
        private IB_CoilCoolingDXTwoStageWithHumidityControlMode_FieldSet() { }

    }
}
