using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_WaterUseEquipmentDefinition : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => throw new NotImplementedException();
        private double peakFlowRate { get; set; } = 0.000063; // m3s
        private static WaterUseEquipmentDefinition NewDefaultOpsObj(Model model) => new WaterUseEquipmentDefinition(model);

        public IB_WaterUseEquipmentDefinition(double PeakFlowRate) : base(NewDefaultOpsObj(new Model()))
        {
        }

        public WaterUseEquipmentDefinition ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            obj.setPeakFlowRate(peakFlowRate);
            return obj;
        }
        public override IB_ModelObject Duplicate()
        {
            var obj = base.Duplicate() as IB_WaterUseEquipmentDefinition;
            obj.peakFlowRate = this.peakFlowRate;

            return obj;
        }
    }
}
