using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingGasMultiStageStageData : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingGasMultiStageStageData();

        private static CoilHeatingGasMultiStageStageData NewDefaultOpsObj(Model model) => new CoilHeatingGasMultiStageStageData(model);

        public IB_CoilHeatingGasMultiStageStageData() : base(NewDefaultOpsObj)
        {
        }
        
        public CoilHeatingGasMultiStageStageData ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_CoilHeatingGasMultiStageStageData_FieldSet
        : IB_FieldSet<IB_CoilHeatingGasMultiStageStageData_FieldSet, CoilHeatingGasMultiStageStageData>
    {
        
        private IB_CoilHeatingGasMultiStageStageData_FieldSet() {
        }

    }
}
