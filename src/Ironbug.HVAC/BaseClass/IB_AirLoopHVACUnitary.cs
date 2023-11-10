using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_AirLoopHVACUnitary : IB_HVACObject, IIB_AirLoopObject
    {

        public IB_AirLoopHVACUnitary(HVACComponent GhostOSObject) : base(GhostOSObject)
        {

        }
    }
}
