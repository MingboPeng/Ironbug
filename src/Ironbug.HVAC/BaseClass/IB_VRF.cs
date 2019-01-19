using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_VRFSystem : IB_ModelObject
    {
        public IB_VRFSystem(HVACComponent GhostOSObject) : base(GhostOSObject)
        {
        }

        public virtual ModelObject ToOS(Model model)
        {
            return this.InitOpsObj(model);
        }
    }
}