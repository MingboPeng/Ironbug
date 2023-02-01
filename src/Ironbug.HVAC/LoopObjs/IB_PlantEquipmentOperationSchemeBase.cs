using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System.Collections.Generic;

namespace Ironbug.HVAC
{
    public abstract class IB_PlantEquipmentOperationSchemeBase : IB_ModelObject
    {
        
        public IB_PlantEquipmentOperationSchemeBase(ModelObject obj) : base(obj)
        {
        }
        protected List<(int limit, IB_HVACObject obj)> _equipments 
        { 
            get => TryGetList<(int limit, IB_HVACObject obj)>(); 
            set => Set(value); 
        }

        public void AddEquipment(int limit, IB_HVACObject obj)
        {
            this._equipments.Add((limit, obj));
        }

        public abstract ModelObject ToOS(PlantLoop loop);

    }

}