using System.Collections.Generic;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public interface IIB_ModelObject
    {
        IB_PuppetableState CurrentState { get; }
        void ChangeState(IB_PuppetableState newState);
        Dictionary<string, object> CustomAttributes { get; }

        IB_ModelObject Duplicate();
        IB_ModelObject DuplicateAsPuppet();
        IEnumerable<string> GetDataFields();
        string GetTrackingID();
        bool IsInModel(Model model);
        void SetAttribute(IB_DataField dataField, object value);
        void SetAttributes(Dictionary<IB_DataField, object> DataFields);
        string SetTrackingID();
        string ToString();
    }
}