using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_EnergyManagementSystemConstructionIndexVariable : IB_EnergyManagementSystemVariable
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_EnergyManagementSystemConstructionIndexVariable();

        private static EnergyManagementSystemConstructionIndexVariable NewDefaultOpsObj(Model model) => new EnergyManagementSystemConstructionIndexVariable(model);
        public IB_EnergyManagementSystemConstructionIndexVariable() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public string ConstructionID { get => Get<string>(); set => Set(value); }

        public override ModelObject ToOS(Model model)
        {
            var oc = model.getConstructionByName(this.ConstructionID);
            if (!oc.is_initialized())
                throw new ArgumentException($"Failed to find the construction {ConstructionID}, you will have to add it to model first.");
            var obj = new EnergyManagementSystemConstructionIndexVariable(model, oc.get());

            return obj;
        }

    }

    public sealed class IB_EnergyManagementSystemConstructionIndexVariable_FieldSet
     : IB_FieldSet<IB_EnergyManagementSystemConstructionIndexVariable_FieldSet, EnergyManagementSystemConstructionIndexVariable>
    {

        private IB_EnergyManagementSystemConstructionIndexVariable_FieldSet() { }

        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");


    }


}
