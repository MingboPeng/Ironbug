using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_Fan : IB_HVACObject, IIB_AirLoopObject
    {
        public IB_Fan(HVACComponent GhostOSObject) : base(GhostOSObject)
        { 

        }
    }
    
}
