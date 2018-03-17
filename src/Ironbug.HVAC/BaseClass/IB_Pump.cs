using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_Pump: IB_HVACObject, IIB_PlantLoopObjects
    {
        public IB_Pump(HVACComponent GhostOSObject) : base(GhostOSObject)
        {
            
        }
    }
}
