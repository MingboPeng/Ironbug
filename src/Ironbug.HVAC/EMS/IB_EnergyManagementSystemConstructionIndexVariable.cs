using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_EnergyManagementSystemConstructionIndexVariable : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_EnergyManagementSystemConstructionIndexVariable();

        private static EnergyManagementSystemConstructionIndexVariable NewDefaultOpsObj(Model model) => new EnergyManagementSystemConstructionIndexVariable(model);
        public IB_EnergyManagementSystemConstructionIndexVariable() : base(NewDefaultOpsObj(new Model()))
        {
        }
        private string _name { get; set; }
        private string _construction { get; set; }
        public void SetName(string name)
        {
            this._name = name;
            var p = this.GhostOSObject;
            p.setName(name);
        }

        public void SetConstructionID(string construction)
        {
            this._construction = construction;
        }


        public EnergyManagementSystemConstructionIndexVariable ToOS(Model model)
        {
            var oc = model.getConstructionByName(this._construction);
            if (!oc.is_initialized())
                throw new ArgumentException($"Failed to find the construction {_construction}, you will have to add it to model first.");
            var obj = new EnergyManagementSystemConstructionIndexVariable(model, oc.get());
            if (string.IsNullOrEmpty(_name))
                obj.setName(_name);
            return obj;
        }

    }

    
}
