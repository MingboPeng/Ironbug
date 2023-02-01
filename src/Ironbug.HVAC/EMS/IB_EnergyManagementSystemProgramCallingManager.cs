using System;
using System.Collections.Generic;
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
    
        public List<IB_EnergyManagementSystemProgram> Programs 
        {
            get => GetList<IB_EnergyManagementSystemProgram>(initDefault: true);
            set => Set(value); 
        }

    
        public EnergyManagementSystemProgramCallingManager ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
      
            foreach (var item in Programs)
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
        public IB_Field Name { get; }
         = new IB_BasicField("Name", "Name");
        public IB_Field CallingPoint { get; }
            = new IB_BasicField("CallingPoint", "CallingPoint");

    }

}
