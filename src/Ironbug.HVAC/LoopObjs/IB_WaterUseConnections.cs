using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC
{
    public class IB_WaterUseConnections : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_WaterUseConnections();
        
        private List<IB_WaterUseEquipment> waterUseEquips { get; set; } = new List<IB_WaterUseEquipment>();
        
        
        private static WaterUseConnections NewDefaultOpsObj(Model model) => new WaterUseConnections(model);
        public IB_WaterUseConnections() : base(NewDefaultOpsObj(new Model()))
        {
        }
        
        public void AddWaterUseEquipment(IB_WaterUseEquipment WaterEquipment)
        {
            this.waterUseEquips.Add(WaterEquipment);
        }

        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            var eqps = this.waterUseEquips;

            foreach (var eqp in eqps)
            {
                obj.addWaterUseEquipment(eqp.ToOS(model));
            }
            return obj;
        }
        

        public override IB_ModelObject Duplicate()
        {

            var newObj = base.Duplicate() as IB_WaterUseConnections;
            var eqps = this.waterUseEquips;

            foreach (var eqp in eqps)
            {
                newObj.AddWaterUseEquipment(eqp.Duplicate() as IB_WaterUseEquipment);
            }
            
            return newObj;
        }

      
    }


    public sealed class IB_WaterUseConnections_FieldSet
        : IB_FieldSet<IB_WaterUseConnections_FieldSet, WaterUseConnections>
    {
        private IB_WaterUseConnections_FieldSet() { }
        
    }



}

