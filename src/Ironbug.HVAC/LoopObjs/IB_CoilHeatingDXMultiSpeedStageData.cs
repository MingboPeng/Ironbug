using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingDXMultiSpeedStageData : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingDXMultiSpeedStageData();

        private static CoilHeatingDXMultiSpeedStageData NewDefaultOpsObj(Model model) => new CoilHeatingDXMultiSpeedStageData(model);

        public IB_CoilHeatingDXMultiSpeedStageData() : base(NewDefaultOpsObj)
        {
        }
        
        public CoilHeatingDXMultiSpeedStageData ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_CoilHeatingDXMultiSpeedStageData_FieldSet
        : IB_FieldSet<IB_CoilHeatingDXMultiSpeedStageData_FieldSet, CoilHeatingDXMultiSpeedStageData>
    {
        
        private IB_CoilHeatingDXMultiSpeedStageData_FieldSet() {
        }

    }
}
