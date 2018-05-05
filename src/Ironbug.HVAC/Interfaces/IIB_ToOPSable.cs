using OpenStudio;

namespace Ironbug.HVAC
{
    public interface IIB_ToOPSable
    {
         ModelObject ToOS(Model model);
    }
}
