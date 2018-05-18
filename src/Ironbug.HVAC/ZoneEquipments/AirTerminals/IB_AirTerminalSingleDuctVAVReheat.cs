using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    //public class IB_AirTerminalSingleDuctVAVReheat: IB_AirTerminal
    //{
    //    //Associated child object
    //    public IB_Coil ReheatCoil { get; private set; } = new IB_CoilHeatingWater();
        
    //    protected Action<IB_Coil> RelationshipsWithChild => SetReheatCoil;

    //    //protected override RelationWithChild<IIB_ModelObject> RelationshipsWithChild => (coil) => this.ReheatCoil = coil;

    //    //private RelationWithChild<IB_Coil> RelationshipsWithChild => SetReheatCoil;


    //    //this is for OpenStudio object initialization
    //    private static AirTerminalSingleDuctVAVReheat InitMethod(Model model) => 
    //        new AirTerminalSingleDuctVAVReheat(model, model.alwaysOnDiscreteSchedule(),new CoilHeatingWater(model));

    //    //this is for self duplication and duplication as Puppet
    //    protected override Func<IB_ModelObject> IB_InitFunc => () => new IB_AirTerminalSingleDuctVAVReheat();

    //    //protected override RelationWithChild<IIB_ModelObject> RelationshipsWithChild => SetReheatCoil;

    //    private static Func<IB_AirTerminalSingleDuctVAVReheat> IB_InitFunc2 = () => new IB_AirTerminalSingleDuctVAVReheat();
        
    //    public IB_AirTerminalSingleDuctVAVReheat():base(InitMethod(new Model()))
    //    {
    //    }
        
    //    public void SetReheatCoil(IB_Coil ReheatCoil)
    //    {
    //        this.ReheatCoil = ReheatCoil;
    //    }
        
    //    public override IB_ModelObject Duplicate()
    //    {
    //        var newObj = base.Duplicate();
    //        var newCoil = (IB_Coil)this.ReheatCoil.Duplicate();
    //        newObj.SetRelationships(RelationshipsWithChild, newCoil);
    //        return newObj;
    //    }
        

    //    public IB_ModelObject DuplicateAsPuppet()
    //    {
    //        //var newObj = (IB_AirTerminalSingleDuctVAVReheat)base.DuplicateIBObj(() => new IB_AirTerminalSingleDuctVAVReheat());
    //        var newObj = this.DuplicateAsPuppet(IB_InitFunc2);
    //        var newCoil = (IB_Coil)this.ReheatCoil.DuplicateAsPuppet();
    //        newObj.SetReheatCoil(newCoil);
    //        return newObj;
    //    }

    //    public  IB_ModelObject Duplicate2()
    //    {
    //        Func<IB_ModelObject> duplicateSelf = ()=> base.DuplicateIBObj(IB_InitFunc);
    //        Func<IB_Coil> duplicateCoil = ()=> this.ReheatCoil.Duplicate() as IB_Coil;
    //        var newObj =  MaintainRelationships(duplicateSelf, duplicateCoil, RelationshipsWithChild);
    //        return newObj;
    //    }

    //    public IB_ModelObject Duplicate3()
    //    {
    //        var newObj = (IB_AirTerminalSingleDuctVAVReheat)base.DuplicateIBObj(IB_InitFunc);
    //        var newCoil = (IB_Coil)this.ReheatCoil.Duplicate();
    //        newObj.SetRelationships(RelationshipsWithChild, newCoil);
    //        return newObj;
    //    }

    //    public IB_ModelObject MaintainRelationships<T>(Func<IB_ModelObject> hostFunc, Func<T> childFunc, Action<T> doAction) where T: IB_ModelObject
    //    {
            
    //        var host = hostFunc();
    //        var child = childFunc();
    //        host.SetRelationships<T>(doAction, child);

    //        return host;
    //    }



    //    protected override ModelObject InitOpsObj(Model model)
    //    {
    //        var newOSObj = base.OnInitOpsObj(InitMethod, model).to_AirTerminalSingleDuctVAVReheat().get();
    //        var newOSCoil = (HVACComponent)this.ReheatCoil.ToOS(model);
    //        newOSObj.setReheatCoil(newOSCoil);//TODO: this one to one assign. need to call puppets' ToOS method as well.

    //        return newOSObj;
    //    }

    //    public override void SetAllTrackingIDs()
    //    {
    //        this.ReheatCoil.SetAllTrackingIDs();
    //        base.SetAllTrackingIDs();
            
    //    }
    //    /// <summary>
    //    /// This is the public entry point to convert IB object to OpenStudio Object.
    //    /// </summary>
    //    /// <param name="model"></param>
    //    /// <returns></returns>
    //    public override ModelObject ToOS(Model model)
    //    {
    //        //Maintaining the puppets,
    //        foreach (var item in this.GetPuppets())
    //        {
    //            ((IB_AirTerminalSingleDuctVAVReheat)item).ToOS(model);
    //        }
    //        return this.InitOpsObj(model);
    //    }
    //}

    public sealed class IB_AirTerminalSingleDuctVAVReheat_DataFieldSet 
        : IB_DataFieldSet<IB_AirTerminalSingleDuctVAVReheat_DataFieldSet, AirTerminalSingleDuctVAVReheat>
    {
        private IB_AirTerminalSingleDuctVAVReheat_DataFieldSet() {}

    }
}
