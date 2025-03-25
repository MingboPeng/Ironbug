using OpenStudio;
using System;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_HVACObject : IB_ModelObject, IIB_HVACObject
    {
        public IB_HVACObject(Func<OpenStudio.Model, HVACComponent> ghostObjInit) : base(ghostObjInit)
        {

        }

        public abstract HVACComponent ToOS(Model model); 
        public virtual bool AddToNode(Model model, Node node)
        {
            return ToOS(model).addToNode(node);
        }



    }


}
