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

        private static EnergyManagementSystemActuator NewDefaultOpsObj(ModelObject obj, string type, string controlType) => new EnergyManagementSystemActuator(obj, type, controlType);
        public IB_EnergyManagementSystemActuator() : base(NewDefaultOpsObj(new Node(new Model()), "component type", "component control type"))
        {
        }
        public IB_EnergyManagementSystemActuator(IB_ModelObject obj, string actuatedComType, string actuatedControlType) : base(NewDefaultOpsObj(new Node(new Model()), actuatedComType, actuatedControlType))
        {
            this.AddChild(obj);
            this._actuatedComponentType = actuatedComType;
            this._actuatedComponentControlType = actuatedControlType;
        }
        private IB_ModelObject _actuatedObj => this.GetChild<IB_ModelObject>(0);
        private string _actuatedComponentType { get; set; }
        private string _actuatedComponentControlType { get; set; }

        public void SetActuatedComponentType(string type)
        {
            this._actuatedComponentType = type;
        }

        public void SetActuatedComponentControlType(string type)
        {
            this._actuatedComponentControlType = type;
        }

        //public override IB_ModelObject Duplicate()
        //{
        //    return base.Duplicate();
        //}

        public EnergyManagementSystemActuator ToOS(Model model)
        {
            var objInModel = _actuatedObj.GetOsmObjInModel(model);
            var obj = base.OnNewOpsObj(InitMethodWithChildren, model);

            return obj;

            EnergyManagementSystemActuator InitMethodWithChildren(Model md)=> new EnergyManagementSystemActuator(objInModel, this._actuatedComponentType, this._actuatedComponentControlType);
        }

        public EnergyManagementSystemActuator ToOS(ModelObject actuatedObj)
        {
            var model = actuatedObj.model();
            var obj = base.OnNewOpsObj(InitMethodWithChildren, model);
            return obj;

            EnergyManagementSystemActuator InitMethodWithChildren(Model md) => new EnergyManagementSystemActuator(actuatedObj, this._actuatedComponentType, this._actuatedComponentControlType);
        }

    }

    public sealed class IB_EnergyManagementSystemActuator_FieldSet
       : IB_FieldSet<IB_EnergyManagementSystemActuator_FieldSet, EnergyManagementSystemActuator>
    {
        private IB_EnergyManagementSystemActuator_FieldSet() { }
    }

}
