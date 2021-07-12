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
        protected List<(int limit, IB_HVACObject obj)> _equipments = new List<(int limit, IB_HVACObject obj)>();

        public override IB_ModelObject Duplicate()
        {
            var obj = base.Duplicate() as IB_PlantEquipmentOperationSchemeBase;
            obj._equipments = this._equipments;
            return obj;
        }

        public void AddEquipment(int limit, IB_HVACObject obj)
        {
            this._equipments.Add((limit, obj));
        }

        public abstract ModelObject ToOS(PlantLoop loop);

    }

}