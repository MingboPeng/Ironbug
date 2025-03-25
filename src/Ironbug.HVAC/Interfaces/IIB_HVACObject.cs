using OpenStudio;

namespace Ironbug.HVAC
{
    public interface IIB_HVACObject : IIB_ModelObject
    {
        HVACComponent ToOS(Model model);
        bool AddToNode(Model model, Node node);
    }
}
