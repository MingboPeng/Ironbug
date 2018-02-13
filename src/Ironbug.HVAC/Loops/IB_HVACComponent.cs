using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public abstract class IB_HVACComponent: IB_ModelObject
    {
        
        //Must override in child class
        public abstract bool AddToNode(ref Model model, Node node);

        //public abstract T ToOS<T>(ref Model model) where T: OpenStudio.HVACComponent;
        public abstract HVACComponent ToOS(ref Model model);
        //public IB_HVACComponent()
        //{

        //}



    }

    


    //public interface IB_Viz
    //{
    //    //base point to draw the object.
    //    Point3d basePoint { get; set; }
    //}
}
