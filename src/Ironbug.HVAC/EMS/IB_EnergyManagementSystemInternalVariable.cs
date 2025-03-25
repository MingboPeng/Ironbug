using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_EnergyManagementSystemInternalVariable : IB_EnergyManagementSystemVariable
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_EnergyManagementSystemInternalVariable();

        private static EnergyManagementSystemInternalVariable NewDefaultOpsObj(Model model) => new EnergyManagementSystemInternalVariable(model, "");
        public IB_EnergyManagementSystemInternalVariable() : base(NewDefaultOpsObj)
        {
        }

        public IB_ModelObject HostObj { get => Get<IB_ModelObject>(); set => Set(value); }

        public void SetInternalDataType(string dataType)
        {
            var f = IB_EnergyManagementSystemInternalVariable_FieldSet.Value;
            this.AddCustomAttribute(f.InternalDataType, dataType);
        }

        public override ModelObject ToOS(Model model)
        {
            var host = HostObj.GetOsmObjInModel(model);
            if (host == null)
                throw new ArgumentException("Failed to find the host object that this internal variable is associated with, you will have to add the host object to model first.");
            
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            obj.setInternalDataIndexKeyName(host.handle().__str__());
            return obj;

        }

        public EnergyManagementSystemInternalVariable ToOS(Model model, ModelObject modelObject)
        {
            var host = modelObject;
            if (host == null)
                throw new ArgumentException("Failed to find the host object that this internal variable is associated with, you will have to add the host object to model first.");
          
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            obj.setInternalDataIndexKeyName(host.handle().__str__());
            return obj;

        }
    }

    public sealed class IB_EnergyManagementSystemInternalVariable_FieldSet
      : IB_FieldSet<IB_EnergyManagementSystemInternalVariable_FieldSet, EnergyManagementSystemInternalVariable>
    {

        private IB_EnergyManagementSystemInternalVariable_FieldSet() { }

        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");

        public IB_Field InternalDataType { get; }
            = new IB_BasicField("InternalDataType", "DataType");

    }

}
