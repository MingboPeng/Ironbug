using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_VRFSystem : IB_HVACObject
    {
        public IB_VRFSystem(HVACComponent GhostOSObject) : base(GhostOSObject)
        {
        }

        //public abstract HVACComponent ToOS(Model model);
       
    }
}