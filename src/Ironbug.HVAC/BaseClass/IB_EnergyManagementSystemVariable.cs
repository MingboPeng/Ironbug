
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_EnergyManagementSystemVariable : IB_ModelObject
    {
        protected IB_EnergyManagementSystemVariable(ModelObject ghostOSObject): base(ghostOSObject)
        {
        }

        public abstract ModelObject ToOS(Model model);
    }

    
}
