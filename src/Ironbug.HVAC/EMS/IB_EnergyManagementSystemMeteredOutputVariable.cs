using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_EnergyManagementSystemMeteredOutputVariable : IB_EnergyManagementSystemVariable
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_EnergyManagementSystemMeteredOutputVariable();

        private static EnergyManagementSystemMeteredOutputVariable NewDefaultOpsObj(Model model) => new EnergyManagementSystemMeteredOutputVariable(model, "Elec");
        public IB_EnergyManagementSystemMeteredOutputVariable() : base(NewDefaultOpsObj(new Model()))
        {
        }
        private IB_EnergyManagementSystemProgram _program => this.GetChild<IB_EnergyManagementSystemProgram>(0);
        public IB_EnergyManagementSystemMeteredOutputVariable(IB_EnergyManagementSystemProgram program) : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(program);
        }
       
        public override ModelObject ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            var p = this._program.GetOsmObjInModel(model) as EnergyManagementSystemProgram;
            obj.setEMSProgramOrSubroutineName(p);
            return obj;
        }

    }
    public sealed class IB_EnergyManagementSystemMeteredOutputVariable_FieldSet
       : IB_FieldSet<IB_EnergyManagementSystemMeteredOutputVariable_FieldSet, EnergyManagementSystemMeteredOutputVariable>
    {
        private IB_EnergyManagementSystemMeteredOutputVariable_FieldSet() { }

    }

}
