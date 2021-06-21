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

        private static EnergyManagementSystemSensor NewDefaultOpsObj(Model model) => new EnergyManagementSystemSensor(model, "");
        public IB_EnergyManagementSystemSensor() : base(NewDefaultOpsObj(new Model()))
        {
        }
      
        public void SetKeyName(string keyName)
        {
            var f = IB_EnergyManagementSystemSensor_FieldSet.Value;
            this.AddCustomAttribute(f.KeyName, keyName);
        }
        public void SetOutputVariable(string outputVariable)
        {
            var f = IB_EnergyManagementSystemSensor_FieldSet.Value;
            this.AddCustomAttribute(f.OutputVariableOrMeterName, outputVariable);
        }

        public EnergyManagementSystemSensor ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            return obj;
        }

    }


    public sealed class IB_EnergyManagementSystemSensor_FieldSet
    : IB_FieldSet<IB_EnergyManagementSystemSensor_FieldSet, EnergyManagementSystemSensor>
    {

        private IB_EnergyManagementSystemSensor_FieldSet() { }

        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");
        public IB_Field OutputVariableOrMeterName { get; }
            = new IB_BasicField("OutputVariableOrMeterName", "OutputVariable");
        public IB_Field KeyName { get; }
          = new IB_BasicField("KeyName", "KeyName");
    }

}
