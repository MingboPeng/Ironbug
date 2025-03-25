
using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_EnergyManagementSystemVariable : IB_ModelObject
    {
        protected IB_EnergyManagementSystemVariable(Func<OpenStudio.Model, ModelObject> ghostObjInit) : base(ghostObjInit)
        {
        }

        public abstract ModelObject ToOS(Model model);
    }

    
}
