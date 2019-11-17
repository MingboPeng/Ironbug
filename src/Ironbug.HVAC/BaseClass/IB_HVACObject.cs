using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_HVACObject : IB_ModelObject, IIB_HVACObject
    {
        public IB_HVACObject(HVACComponent GhostOSObject) : base(GhostOSObject)
        {

        }

        public abstract HVACComponent ToOS(Model model); 
        public virtual bool AddToNode(Node node)
        {
            var model = node.model();
            return ToOS(model).addToNode(node);
        }



    }


}
