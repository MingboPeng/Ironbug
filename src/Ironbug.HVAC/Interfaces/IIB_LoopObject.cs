using OpenStudio;

namespace Ironbug.HVAC
{
    public interface IIB_LoopObject
    {
        bool AddToNode(Node node);
    }
}
