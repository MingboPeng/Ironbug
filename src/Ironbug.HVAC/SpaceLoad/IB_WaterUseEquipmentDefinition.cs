using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Runtime.Serialization;

namespace Ironbug.HVAC
{
    public class IB_WaterUseEquipmentDefinition : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_WaterUseEquipmentDefinition(this.PeakFlowRate);

        private double PeakFlowRate { get => Get(0.0); set => Set(value, 0.0); } // m3s
        private static WaterUseEquipmentDefinition NewDefaultOpsObj(Model model) => new WaterUseEquipmentDefinition(model);

        private IB_WaterUseEquipmentDefinition() : base(null) { }
        public IB_WaterUseEquipmentDefinition(double PeakFlowRate = 0.000063) : base(NewDefaultOpsObj(new Model()))
        {
            this.PeakFlowRate = PeakFlowRate;
        }

        public WaterUseEquipmentDefinition ToOS(Model model)
        {
            var name = $"WaterUseLoad {PeakFlowRate} m3/s ({Math.Round(PeakFlowRate* 15850.372483753,1)} gpm)";
            var optionalObj = model.getWaterUseEquipmentDefinitionByName(name);
            if (optionalObj.is_initialized())
            {
                return optionalObj.get();
            }
            else
            {
                var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
                obj.setPeakFlowRate(PeakFlowRate);
                obj.setName(name);
                return obj;
            }
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
