//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Ironbug.HVAC.BaseClass;
//using OpenStudio;

//namespace Ironbug.HVAC
//{
//    public class IB_EnergyManagementSystemMeteredOutputVariable : IB_ModelObject
//    {
//        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_EnergyManagementSystemMeteredOutputVariable();

//        private static EnergyManagementSystemMeteredOutputVariable NewDefaultOpsObj(Model model) => new EnergyManagementSystemMeteredOutputVariable(model);
//        public IB_EnergyManagementSystemMeteredOutputVariable() : base(NewDefaultOpsObj(new Model()))
//        {
//        }
//        private string _name { get; set; }
//        private IB_MeteredOutput _MeteredOutput { get; set; }
//        public void SetName(string name)
//        {
//            this._name = name;
//            var p = this.GhostOSObject;
//            p.setName(name);
//        }

//        public void SetMeteredOutput(IB_MeteredOutput _MeteredOutput)
//        {
//            this._MeteredOutput = _MeteredOutput;
//        }


//        public EnergyManagementSystemMeteredOutputVariable ToOS(Model model)
//        {
//            var MeteredOutput = _MeteredOutput.GetOsmObjInModel(model) as MeteredOutput;
//            var obj = new EnergyManagementSystemMeteredOutputVariable(model, MeteredOutput);
//            if (string.IsNullOrEmpty(_name))
//                obj.setName(_name);
//            return obj;
//        }

//    }

    
//}
