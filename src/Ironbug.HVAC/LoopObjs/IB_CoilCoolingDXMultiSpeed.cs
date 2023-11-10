using System;
using System.Collections.Generic;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingDXMultiSpeed : IB_CoilDX
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingDXMultiSpeed();

        private static CoilCoolingDXMultiSpeed NewDefaultOpsObj(Model model) => new CoilCoolingDXMultiSpeed(model);

        public List<IB_CoilCoolingDXMultiSpeedStageData> Stages
        {
            get => this.TryGetList<IB_CoilCoolingDXMultiSpeedStageData>();
            private set => this.Set(value);
        }

        public IB_CoilCoolingDXMultiSpeed() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public void SetStages(List<IB_CoilCoolingDXMultiSpeedStageData> stages)
        {
            this.Stages = stages;
        }

        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            foreach (var stage in Stages)
            {
                obj.addStage(stage.ToOS(model));
            }
            return obj;
        }

    }

    public sealed class IB_CoilCoolingDXMultiSpeed_FieldSet
        : IB_FieldSet<IB_CoilCoolingDXMultiSpeed_FieldSet, CoilCoolingDXMultiSpeed>
    {
        private IB_CoilCoolingDXMultiSpeed_FieldSet() { }

    }
}
