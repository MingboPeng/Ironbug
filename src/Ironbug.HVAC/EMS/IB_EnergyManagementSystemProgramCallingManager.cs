using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_EnergyManagementSystemProgramCallingManager : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_EnergyManagementSystemProgramCallingManager();

        private static EnergyManagementSystemProgramCallingManager NewDefaultOpsObj(Model model) => new EnergyManagementSystemProgramCallingManager(model);
        public IB_EnergyManagementSystemProgramCallingManager() : base(NewDefaultOpsObj(new Model()))
        {
        }
        private string _name { get; set; }
        private List<IB_EnergyManagementSystemProgram> _programs { get; set; }

        public void SetName(string name)
        {
            this._name = name;
            var p = this.GhostOSObject as EnergyManagementSystemProgram;
            p.setName(name);
        }

        public void SetPrograms(List<IB_EnergyManagementSystemProgram> programs)
        {
            this._programs = programs;
            //var p = this.GhostOSObject as EnergyManagementSystemProgramCallingManager;
            //foreach (var item in programs)
            //{
            //    p.addProgram(item.ToOS);
            //}
        }

        public EnergyManagementSystemProgramCallingManager ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            if (string.IsNullOrEmpty(_name))
                obj.setName(_name);
            foreach (var item in _programs)
            {
                var prg = item.GetOsmObjInModel(model) as EnergyManagementSystemProgram;
                if (prg == null)
                    throw new ArgumentException("Failed to find the program in model, you will have to add the program to model first.");
                obj.addProgram(prg);
            }
            return obj;
        }

    }
    public sealed class IB_EnergyManagementSystemProgramCallingManager_FieldSet
       : IB_FieldSet<IB_EnergyManagementSystemProgramCallingManager_FieldSet, EnergyManagementSystemProgramCallingManager>
    {
        private IB_EnergyManagementSystemProgramCallingManager_FieldSet() { }
        public IB_Field CallingPoint { get; }
       = new IB_BasicField("CallingPoint", "CallingPoint");

    }

}
