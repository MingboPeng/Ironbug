using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_WaterUseEquipment : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_WaterUseEquipment(this.waterUseLoad);

        private IB_WaterUseEquipmentDefinition waterUseLoad => this.Children.Get<IB_WaterUseEquipmentDefinition>();
        private string spaceName { get; set; } = string.Empty;

        private static WaterUseEquipment NewDefaultOpsObj(Model model) => new WaterUseEquipment(new WaterUseEquipmentDefinition(model));
        public IB_WaterUseEquipment(IB_WaterUseEquipmentDefinition waterUseLoad) : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(waterUseLoad);
            
        }
        public void SetSpace(string ZoneName)
        {
            this.spaceName = ZoneName;

        }
      
        public WaterUseEquipment ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(localMethod, model); 
            if (!string.IsNullOrEmpty(spaceName))
            {
                var optionalSpace = model.getSpaceByName($"{spaceName}_space");
                if (optionalSpace.is_initialized())
                {
                    obj.setSpace(optionalSpace.get());
                }
            }

            return obj;

            WaterUseEquipment localMethod(Model m)
            {
                return new WaterUseEquipment(this.waterUseLoad.ToOS(m));
            }

        }
        public override IB_ModelObject Duplicate()
        {
            var obj = base.Duplicate() as IB_WaterUseEquipment;
            obj.spaceName = this.spaceName;

            return obj;
        }
    }
    public sealed class IB_WaterUseEquipment_FieldSet
        : IB_FieldSet<IB_WaterUseEquipment_FieldSet, WaterUseEquipment>
    {
        private IB_WaterUseEquipment_FieldSet() { }
    }
}
