using System;
using System.Collections.Generic;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingDXMultiSpeed : IB_CoilDX
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingDXMultiSpeed();

        private static CoilHeatingDXMultiSpeed NewDefaultOpsObj(Model model) => new CoilHeatingDXMultiSpeed(model);

        public List<IB_CoilHeatingDXMultiSpeedStageData> Stages
        {
            get => this.TryGetList<IB_CoilHeatingDXMultiSpeedStageData>();
            private set => this.Set(value);
        }


        public IB_CoilHeatingDXMultiSpeed() : base(NewDefaultOpsObj)
        {
            
        }
        
        public void SetStages(List<IB_CoilHeatingDXMultiSpeedStageData> stages)
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

    public sealed class IB_CoilHeatingDXMultiSpeed_FieldSet
        : IB_FieldSet<IB_CoilHeatingDXMultiSpeed_FieldSet, CoilHeatingDXMultiSpeed>
    {
        private IB_CoilHeatingDXMultiSpeed_FieldSet() { }

    }
}
