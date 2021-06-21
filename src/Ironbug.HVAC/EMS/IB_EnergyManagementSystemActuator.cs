using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_EnergyManagementSystemActuator : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_EnergyManagementSystemActuator();

        private static EnergyManagementSystemActuator NewDefaultOpsObj(ModelObject obj) => new EnergyManagementSystemActuator(obj, "", "");
        public IB_EnergyManagementSystemActuator() : base(NewDefaultOpsObj(new Node(new Model())))
        {
        }
        private IB_ModelObject _actuatedObj => this.GetChild<IB_ModelObject>(0);
        public IB_EnergyManagementSystemActuator(IB_ModelObject actuatedObj) : base(NewDefaultOpsObj(new Node(new Model())))
        {
            this.AddChild(actuatedObj);
        }
        public EnergyManagementSystemActuator ToOS(Model model)
        {
            var objInModel = _actuatedObj.GetOsmObjInModel(model);
            if (objInModel == null)
                throw new ArgumentException("Actuated object has not been added to model, please add it first");
            var obj = base.OnNewOpsObj(InitMethodWithChildren, model);

            return obj;

            EnergyManagementSystemActuator InitMethodWithChildren(Model md)=> new EnergyManagementSystemActuator(objInModel, "", "");
        }

        public EnergyManagementSystemActuator ToOS(ModelObject actuatedObj)
        {
            var model = actuatedObj.model();
            var obj = base.OnNewOpsObj(InitMethodWithChildren, model);
            return obj;

            EnergyManagementSystemActuator InitMethodWithChildren(Model md) => new EnergyManagementSystemActuator(actuatedObj, "", "");
        }

    }

    public sealed class IB_EnergyManagementSystemActuator_FieldSet
       : IB_FieldSet<IB_EnergyManagementSystemActuator_FieldSet, EnergyManagementSystemActuator>
    {
        private IB_EnergyManagementSystemActuator_FieldSet() { }
        public IB_Field ActuatedComponentControlType { get; }
            = new IB_BasicField("ActuatedComponentControlType", "ActuatedComponentControlType") { };
        public IB_Field ActuatedComponentType { get; }
            = new IB_BasicField("ActuatedComponentType", "ActuatedComponentType"){};
    }

}
