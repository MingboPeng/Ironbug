using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_CoilDX : IB_Coil
    {
        public IB_CoilDX(HVACComponent GhostOSObject) : base(GhostOSObject)
        {

        }
    }

}
