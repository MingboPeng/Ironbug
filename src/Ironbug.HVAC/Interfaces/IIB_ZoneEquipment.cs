using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public interface IIB_ZoneEquipment: IIB_ModelObject
    {
        ModelObject ToOS(Model model);
    }
}