using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_EnergyManagementSystemSensor : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_EnergyManagementSystemSensor();

        private static EnergyManagementSystemSensor NewDefaultOpsObj(Model model, string outputVariable) => new EnergyManagementSystemSensor(model, outputVariable);
        public IB_EnergyManagementSystemSensor() : base(NewDefaultOpsObj(new Model(), ""))
        {
        }
        public IB_EnergyManagementSystemSensor(string outputVariable) : base(NewDefaultOpsObj(new Model(), outputVariable))
        {
            this._outputVariable = outputVariable;
        }
        private string _name { get; set; }
        private string _outputVariable { get; set; }
        private string _keyName { get; set; }
        public void SetName(string name)
        {
            this._name = name;
            var p = this.GhostOSObject as EnergyManagementSystemSensor;
            p.setName(name);
        }

        public void SetOutputVariableOrMeterName(string outputVariable)
        {
            this._outputVariable = outputVariable;
            var p = this.GhostOSObject as EnergyManagementSystemSensor;
            p.setOutputVariableOrMeterName(outputVariable);
        }

        public void SetKeyName(string keyName)
        {
            this._keyName = keyName;
            var p = this.GhostOSObject as EnergyManagementSystemSensor;
            p.setKeyName(keyName);
        }

        public EnergyManagementSystemSensor ToOS(Model model)
        {
            var obj = new EnergyManagementSystemSensor(model, this._outputVariable);
            if (string.IsNullOrEmpty(_name))
                obj.setName(_name);
            obj.setOutputVariableOrMeterName(this._outputVariable);
            obj.setKeyName(this._keyName);
            return obj;
        }

    }

    
}
