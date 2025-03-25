using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingDXMultiSpeedStageData : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingDXMultiSpeedStageData();

        private static CoilCoolingDXMultiSpeedStageData NewDefaultOpsObj(Model model) => new CoilCoolingDXMultiSpeedStageData(model);

        public IB_CoilCoolingDXMultiSpeedStageData() : base(NewDefaultOpsObj)
        {
        }
        
        public CoilCoolingDXMultiSpeedStageData ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_CoilCoolingDXMultiSpeedStageData_FieldSet
        : IB_FieldSet<IB_CoilCoolingDXMultiSpeedStageData_FieldSet, CoilCoolingDXMultiSpeedStageData>
    {
        
        private IB_CoilCoolingDXMultiSpeedStageData_FieldSet() {
        }

    }
}
