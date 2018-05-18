using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    //public abstract class IB_PuppetableObj : IB_ModelObject, IIB_Puppetable
    //{
    //    public virtual IList<IIB_Puppetable> Puppets { get; private set; } = new List<IIB_Puppetable>();
    //    //private IList<IIB_Puppetable> puppets = new List<IIB_Puppetable>();
    //    //public virtual IList<IIB_Puppetable> Puppets
    //    //{
    //    //    get
    //    //    {
    //    //        if (this.puppets.Count > 0)
    //    //        {
    //    //            return puppets;
    //    //        }
    //    //        else
    //    //        {
    //    //            return new List<IIB_Puppetable>() { this };
    //    //        };
    //    //    }
    //    //    private set { puppets = value }
    //    //}


    //    public IB_PuppetableObj(ModelObject GhostOSObject) : base(GhostOSObject)
    //    {

    //    }

    //    /// <summary>
    //    /// Call this first whenever the puppet is needed.
    //    /// </summary>
    //    public void CreatePuppetPlaceholders()
    //    {
    //        this.Puppets = new List<IIB_Puppetable>();
    //    }


        
    //    public virtual ModelObject ToOS(Model model)
    //    {

    //        if (this.Puppets.Count > 0)
    //        {
    //            foreach (var puppet in Puppets)
    //            {
    //                //each puppet has its own tracking ID, 
    //                //I have to call puppet's ToOS method.
    //                puppet.ToOS(model);
    //            }
    //        }

    //        return this.InitOpsObj(model);
    //    }

    //    //public ModelObject ToOS2(Model model, Func<Model,ModelObject> doToOS)
    //    //{

    //    //    if (this.Puppets.Count > 0)
    //    //    {
    //    //        foreach (var puppet in Puppets)
    //    //        {
    //    //            //each puppet has its own tracking ID, 
    //    //            //I have to call puppet's ToOS method.
    //    //            puppet.ToOS(model);
    //    //        }
    //    //    }

    //    //    var aa = doToOS(model);

    //    //    return this.InitOpsObj(model);
    //    //}

    //    /// <summary>
    //    /// call CreatePuppetPlaceholders() first before using DuplicateAsPuppet()
    //    /// </summary>
    //    /// <returns></returns>
    //    public IIB_Puppetable DuplicateAsPuppet()
    //    {

    //        var puppet = (IB_PuppetableObj)this.Duplicate();
    //        puppet.SetTrackingID();
    //        this.Puppets.Add(puppet);
    //        return puppet;
    //    }
        
    //}
    //public abstract class IB_PuppetableObj<T>
    //    : IB_PuppetableObj
    //    where T : IB_PuppetableObj<T>
    //    //, IIB_Puppetable<T>
    //{
    //    public new virtual IList<T> Puppets { get; private set; } = new List<T>();

    //    public IB_PuppetableObj(ModelObject GhostOSObject) : base(GhostOSObject)
    //    {

    //    }



    //    /// <summary>
    //    /// call CreatePuppetPlaceholders() first before using DuplicateAsPuppet()
    //    /// </summary>
    //    /// <returns></returns>
    //    public new T DuplicateAsPuppet()
    //    {

    //        var puppet = this.Duplicate() as T;
    //        puppet.SetTrackingID();
    //        this.Puppets.Add(puppet);
    //        return puppet;
    //    }



    //}
}
