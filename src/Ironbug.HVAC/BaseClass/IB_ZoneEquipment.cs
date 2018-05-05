using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_ZoneEquipment : IB_ModelObject, IIB_ToOPSable, IIB_ZoneEquipment
    {
        public abstract ModelObject ToOS(Model model);

        public IB_ZoneEquipment(HVACComponent GhostOSObject) : base(GhostOSObject)
        {

        }
    }
}
