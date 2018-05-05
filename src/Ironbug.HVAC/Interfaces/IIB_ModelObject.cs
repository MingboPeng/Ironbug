using System.Collections.Generic;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public interface IIB_ModelObject
    {
        Dictionary<string, object> CustomAttributes { get; }

        IB_ModelObject Duplicate();
        IEnumerable<string> GetDataFields();
        object GetDataFieldValue(string DataFieldName);
        string GetTrackingID();
        bool IsInModel(Model model);
        void SetAttribute(IB_DataField dataField, object value);
        void SetAttributes(Dictionary<IB_DataField, object> DataFields);
        string SetTrackingID();
        string ToString();
    }
}