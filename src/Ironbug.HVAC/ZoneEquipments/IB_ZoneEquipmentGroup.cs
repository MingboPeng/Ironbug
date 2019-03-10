using Ironbug.HVAC.BaseClass;
using System.Collections.Generic;

namespace Ironbug.HVAC
{
    public class IB_ZoneEquipmentGroup
    {
        public List<IB_ZoneEquipment> ZoneEquipments { get; private set; }

        public IB_ZoneEquipmentGroup()
        {
            this.ZoneEquipments = new List<IB_ZoneEquipment>();
        }

        public IB_ZoneEquipmentGroup(List<IB_ZoneEquipment> ZoneEquipments)
        {
            this.ZoneEquipments = ZoneEquipments;
        }

        public void Add(IB_ZoneEquipment ZoneEquipment)
        {
            this.ZoneEquipments.Add(ZoneEquipment);
        }

        public override string ToString()
        {
            return $"Zone Equipment Group with {this.ZoneEquipments.Count} obj(s) inside";
        }
    }
}