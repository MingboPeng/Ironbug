using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_WaterUseEquipmentDefinition : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_WaterUseEquipmentDefinition(this.peakFlowRate);
        private double peakFlowRate { get; set; } = 0; // m3s
        private static WaterUseEquipmentDefinition NewDefaultOpsObj(Model model) => new WaterUseEquipmentDefinition(model);

        public IB_WaterUseEquipmentDefinition(double PeakFlowRate = 0.000063) : base(NewDefaultOpsObj(new Model()))
        {
            this.peakFlowRate = PeakFlowRate;
        }

        public WaterUseEquipmentDefinition ToOS(Model model)
        {
            var name = $"WaterUseLoad {peakFlowRate} m3/s ({Math.Round(peakFlowRate* 15850.372483753,1)} gpm)";
            var optionalObj = model.getWaterUseEquipmentDefinitionByName(name);
            if (optionalObj.is_initialized())
            {
                return optionalObj.get();
            }
            else
            {
                var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
                obj.setPeakFlowRate(peakFlowRate);
                obj.setName(name);
                return obj;
            }
        }
        public override IB_ModelObject Duplicate()
        {
            var obj = base.Duplicate() as IB_WaterUseEquipmentDefinition;
            obj.peakFlowRate = this.peakFlowRate;

            return obj;
        }
    }

    public sealed class IB_WaterUseEquipmentDefinition_FieldSet
      : IB_FieldSet<IB_WaterUseEquipmentDefinition_FieldSet>
    {
        private IB_WaterUseEquipmentDefinition_FieldSet()
        {
        }

        public IB_Field PeakFlowRate { get; }
            = new IB_TopField("PeakFlowRate", "PeakFlowRate");

    }
}
