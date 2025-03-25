using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC
{
    public class IB_WaterUseConnections : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_WaterUseConnections();

     
        public List<IB_WaterUseEquipment> WaterUseEquips 
        { 
            get => TryGetList<IB_WaterUseEquipment>(); 
            set => Set(value); 
        } 
        
        private static WaterUseConnections NewDefaultOpsObj(Model model) => new WaterUseConnections(model);
        public IB_WaterUseConnections() : base(NewDefaultOpsObj)
        {
        }
        

        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            var eqps = this.WaterUseEquips;

            foreach (var eqp in eqps)
            {
                obj.addWaterUseEquipment(eqp.ToOS(model));
            }
            return obj;
        }
        
    }


    public sealed class IB_WaterUseConnections_FieldSet
        : IB_FieldSet<IB_WaterUseConnections_FieldSet, WaterUseConnections>
    {
        private IB_WaterUseConnections_FieldSet() { }
        
    }



}

