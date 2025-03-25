using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System.Collections.Generic;

namespace Ironbug.HVAC
{
    public class IB_ZoneEquipmentGroup: IIB_ZoneEquipment
    {
        public List<IB_ZoneEquipment> ZoneEquipments { get; private set; }

        public IB_FieldArgumentSet CustomAttributes => throw new System.NotImplementedException();

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


        public HVACComponent ToOS(Model model)
        {
            throw new System.NotImplementedException();
        }

        public bool AddToNode(Model model, Node node)
        {
            throw new System.NotImplementedException();
        }

        public IB_ModelObject Duplicate()
        {
            throw new System.NotImplementedException();
        }

        public string GetTrackingID()
        {
            throw new System.NotImplementedException();
        }

        public bool IsInModel(Model model)
        {
            throw new System.NotImplementedException();
        }

        public void SetFieldValue(IB_Field dataField, object value)
        {
            throw new System.NotImplementedException();
        }

        public void SetFieldValues(Dictionary<IB_Field, object> DataFields)
        {
            throw new System.NotImplementedException();
        }

        public string SetTrackingID(string id = null)
        {
            throw new System.NotImplementedException();
        }
    }
}