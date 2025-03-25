using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC
{
    public abstract class IB_PlantEquipmentOperationSchemeBase : IB_ModelObject
    {
        
        public IB_PlantEquipmentOperationSchemeBase(Func<OpenStudio.Model, ModelObject> ghostObjInit) : base(ghostObjInit)
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

        public abstract ModelObject ToOS(Model model, PlantLoop loop);

    }

}