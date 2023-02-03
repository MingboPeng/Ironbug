using System;
using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_WaterUseEquipment : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_WaterUseEquipment(this.waterUseLoad);

        private IB_WaterUseEquipmentDefinition waterUseLoad => this.GetChild<IB_WaterUseEquipmentDefinition>();
 
        private string SpaceName { get => Get<string>(); set => Set(value); }

        private static WaterUseEquipment NewDefaultOpsObj(Model model) => new WaterUseEquipment(new WaterUseEquipmentDefinition(model));
       
        [JsonConstructor]
        private IB_WaterUseEquipment() : base(null) { }
        public IB_WaterUseEquipment(IB_WaterUseEquipmentDefinition waterUseLoad) : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(waterUseLoad);
            
        }
        public void SetSpace(string ZoneName)
        {
            this.SpaceName = ZoneName;

        }
      
        public WaterUseEquipment ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(localMethod, model); 
            if (!string.IsNullOrEmpty(SpaceName))
            {
                var optionalSpace = model.getSpaceByName($"{SpaceName}_space");
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
    
    }
    public sealed class IB_WaterUseEquipment_FieldSet
        : IB_FieldSet<IB_WaterUseEquipment_FieldSet, WaterUseEquipment>
    {
        private IB_WaterUseEquipment_FieldSet() { }
    }
}
