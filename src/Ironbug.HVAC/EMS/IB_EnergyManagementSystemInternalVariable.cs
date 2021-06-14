using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_EnergyManagementSystemInternalVariable : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_EnergyManagementSystemInternalVariable();

        private static EnergyManagementSystemInternalVariable NewDefaultOpsObj(Model model, string dataType) => new EnergyManagementSystemInternalVariable(model, dataType);
        public IB_EnergyManagementSystemInternalVariable() : base(NewDefaultOpsObj(new Model(), ""))
        {
        }
        private string _name { get; set; }
        private string _internalDataType { get; set; }
        private IB_ModelObject _hostObj { get; set; }
        //private string _keyName { get; set; }
        public void SetName(string name)
        {
            this._name = name;
            var p = this.GhostOSObject as EnergyManagementSystemInternalVariable;
            p.setName(name);
        }

        public void SetInternalDataType(string internalDataType)
        {
            this._internalDataType = internalDataType;
            var p = this.GhostOSObject as EnergyManagementSystemInternalVariable;
            p.setInternalDataType(internalDataType);
        }
        public void SetHostObj(IB_ModelObject host)
        {
            this._hostObj = host;
        }

        //public void SetKeyName(string keyName)
        //{
        //    this._keyName = keyName;
        //    var p = this.GhostOSObject as EnergyManagementSystemInternalVariable;
        //    p.setInternalDataIndexKeyName(keyName);
        //}

        public EnergyManagementSystemInternalVariable ToOS(Model model)
        {
            var host = _hostObj.GetOsmObjInModel(model);
            if (host == null)
                throw new ArgumentException("Failed to find the host object that this internal variable is associated with, you will have to add the host object to model first.");
            var obj = new EnergyManagementSystemInternalVariable(model, this._internalDataType);
            if (string.IsNullOrEmpty(_name))
                obj.setName(_name);
            obj.setInternalDataType(this._internalDataType);
            obj.setInternalDataIndexKeyName(host.handle().ToString());
            return obj;
        }
        public EnergyManagementSystemInternalVariable ToOS(ModelObject modelObject)
        {
            var host = modelObject;
            if (host == null)
                throw new ArgumentException("Failed to find the host object that this internal variable is associated with, you will have to add the host object to model first.");
            var obj = new EnergyManagementSystemInternalVariable(host.model(), this._internalDataType);
            if (string.IsNullOrEmpty(_name))
                obj.setName(_name);
            obj.setInternalDataType(this._internalDataType);
            obj.setInternalDataIndexKeyName(host.handle().ToString());
            return obj;
        }
    }

    
}
