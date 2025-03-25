using OpenStudio;
using System;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_ZoneEquipment : IB_HVACObject, IIB_ZoneEquipment
    {
        public IB_ZoneEquipment(Func<OpenStudio.Model, HVACComponent> ghostObjInit) : base(ghostObjInit)
        {
        }

        //public abstract ModelObject ToOS(Model model);

        //public override bool AddToNode(Node node)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public override IB_ModelObject Duplicate()
        //{
        //    var newObj = base.DuplicateIBObj(IB_InitSelf);

        //    foreach (var child in this.Children)
        //    {
        //        newObj.Children.Add(child.Duplicate());
        //    }

        //    return newObj;
        //}

        //public override IB_ModelObject DuplicateAsPuppet()
        //{
        //    var newObj = base.DuplicateAsPuppet(IB_InitSelf);

        //    foreach (var child in this.Children)
        //    {
        //        var childPuppet = child.DuplicateAsPuppet();
        //        newObj.Children.Add(childPuppet);
        //    }

        //    return newObj;
        //}
    }

    //public abstract class IB_ZoneEquipment<T> : IB_PuppetableObj<T>, IIB_ZoneEquipment<T>
    //{
    //    public IB_ZoneEquipment(HVACComponent GhostOSObject) : base(GhostOSObject)
    //    {
    //    }

    //    //public abstract ModelObject ToOS(Model model);

    //}
}