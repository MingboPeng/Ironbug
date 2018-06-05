using System.Collections.Generic;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public interface IIB_ModelObject
    {
        IB_PuppetableState CurrentState { get; }
        void ChangeState(IB_PuppetableState newState);
        Dictionary<IB_Field, object> CustomAttributes { get; }

        IB_ModelObject Duplicate();
        IB_ModelObject DuplicateAsPuppet();
        //IEnumerable<string> GetDataFields();
        string GetTrackingID();
        bool IsInModel(Model model);
        void SetFieldValue(IB_Field dataField, object value);
        void SetFieldValues(Dictionary<IB_Field, object> DataFields);
        string SetTrackingID();
        string ToString();
    }
}