using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_HVACObject : IB_ModelObject, IIB_ToOPSable
    {

        //Must override in child class
        public abstract bool AddToNode(Node node);

        public abstract ModelObject ToOS(Model model);

        public IB_HVACObject(HVACComponent GhostOSObject) : base(GhostOSObject)
        {

        }
        


    }




    //public interface IB_Viz
    //{
    //    //base point to draw the object.
    //    Point3d basePoint { get; set; }
    //}
}
