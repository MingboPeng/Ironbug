using System;
using System.Collections.Generic;
using System.Linq;
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

  
        private string _construction { get; set; }
       
        public void SetConstructionID(string construction)
        {
            this._construction = construction;
        }

        public override IB_ModelObject Duplicate()
        {
            var dup = base.Duplicate() as IB_EnergyManagementSystemConstructionIndexVariable;
            dup._construction = this._construction;
            return dup;
        }
        public override ModelObject ToOS(Model model)
        {
            var oc = model.getConstructionByName(this._construction);
            if (!oc.is_initialized())
                throw new ArgumentException($"Failed to find the construction {_construction}, you will have to add it to model first.");
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
