using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_HVACObject : IB_ModelObject
    {
        //protected abstract Func<IB_ModelObject> IB_InitFunc { get; }

        //Must override in child class
        //public abstract bool AddToNode(Node node);

        //public abstract ModelObject ToOS(Model model);

        public IB_HVACObject(HVACComponent GhostOSObject) : base(GhostOSObject)
        {

        }

        public abstract HVACComponent ToOS(Model model);
        //public virtual HVACComponent ToOS(Model model)
        //{
        //    return this.NewOpsObj(model) as HVACComponent;
        //}

        public bool AddToNode(Node node)
        {
            var model = node.model();
            return ToOS(model).addToNode(node);
        }

        public new virtual IB_HVACObject Duplicate()
        {
            return base.Duplicate() as IB_HVACObject;
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
