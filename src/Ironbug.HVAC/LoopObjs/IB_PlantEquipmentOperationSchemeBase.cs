using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC
{
    public abstract class IB_PlantEquipmentOperationSchemeBase : IB_ModelObject
    {
        public class PlantEquipmentOperationSchemeItem : IEquatable<PlantEquipmentOperationSchemeItem>
        {
            public int Limit { get; set; }
            public IB_HVACObject Obj { get; set; }

            public override bool Equals(object obj)
            {
                return Equals(obj as PlantEquipmentOperationSchemeItem);
            }

            public bool Equals(PlantEquipmentOperationSchemeItem other)
            {
                return other != null &&
                       Limit == other.Limit &&
                       Obj == other.Obj;
            }

            public override int GetHashCode()
            {
                unchecked // Overflow is fine, just wrap
                {
                    int hash = 17;
                    hash = hash * 23 + Limit.GetHashCode();
                    hash = hash * 23 + Obj.GetHashCode();
                    return hash;
                }
            }


        }

        public IB_PlantEquipmentOperationSchemeBase(Func<OpenStudio.Model, ModelObject> ghostObjInit) : base(ghostObjInit)
        {
        }

        protected List<PlantEquipmentOperationSchemeItem> _equipments
        {
            get => TryGetList<PlantEquipmentOperationSchemeItem>();
            set => Set(value);
        }

        public void AddEquipment(int limit, IB_HVACObject obj)
        {
            this._equipments.Add(new PlantEquipmentOperationSchemeItem { Limit = limit, Obj = obj });
        }

        public abstract ModelObject ToOS(Model model, PlantLoop loop);

    }

}