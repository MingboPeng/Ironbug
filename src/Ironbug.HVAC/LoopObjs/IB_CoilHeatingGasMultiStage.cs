using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingGasMultiStage : IB_CoilHeatingBasic
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingGasMultiStage();

        private static CoilHeatingGasMultiStage NewDefaultOpsObj(Model model) => new CoilHeatingGasMultiStage(model);

        public List<IB_CoilHeatingGasMultiStageStageData> Stages
        {
            get => this.TryGetList<IB_CoilHeatingGasMultiStageStageData>();
            set => this.Set(value);
        }

        public IB_CoilHeatingGasMultiStage() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public void SetStages(List<IB_CoilHeatingGasMultiStageStageData> stages)
        {
            this.Stages = stages;
        }

        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);

            foreach (var item in Stages)
            {
                var s = item.ToOS(model);
                obj.addStage(s);
            }
            return obj;
        }
    }

    public sealed class IB_CoilHeatingGasMultiStage_FieldSet
        : IB_FieldSet<IB_CoilHeatingGasMultiStage_FieldSet, CoilHeatingGasMultiStage>
    {
        private IB_CoilHeatingGasMultiStage_FieldSet() {
        }

    }
}
