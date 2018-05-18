using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_HVACObject : IB_ModelObject, IIB_ToOPSable
    {
        //protected abstract Func<IB_ModelObject> IB_InitFunc { get; }

        //Must override in child class
        public abstract bool AddToNode(Node node);

        //public abstract ModelObject ToOS(Model model);

        public IB_HVACObject(HVACComponent GhostOSObject) : base(GhostOSObject)
        {

        }

        public virtual ModelObject ToOS(Model model)
        {
            return this.InitOpsObj(model);
        }
        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(IB_InitSelf);
        }
        //public virtual IB_ModelObject DuplicateAsPuppet()
        //{
        //    return this.DuplicateAsPuppet(IB_InitFunc);
        //}



    }




    //public interface IB_Viz
    //{
    //    //base point to draw the object.
    //    Point3d basePoint { get; set; }
    //}
}
