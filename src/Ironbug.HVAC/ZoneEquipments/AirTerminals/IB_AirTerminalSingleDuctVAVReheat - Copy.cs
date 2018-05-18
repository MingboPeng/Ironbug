using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctVAVReheat: IB_AirTerminal
    {
        //this is for self duplication and duplication as Puppet
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctVAVReheat();
        //this is for OpenStudio object initialization
        private static AirTerminalSingleDuctVAVReheat InitMethod(Model model) =>
            new AirTerminalSingleDuctVAVReheat(model, model.alwaysOnDiscreteSchedule(), new CoilHeatingWater(model));

        private AirTerminalSingleDuctVAVReheat InitMethodWithCoil(Model model) =>
            new AirTerminalSingleDuctVAVReheat(model, model.alwaysOnDiscreteSchedule(), (HVACComponent)this.ReheatCoil.Get<IB_Coil>().ToOS(model));

        //Associated child object 
        //optional if there is no child 
        private IB_Child ReheatCoil => this.Children.GetChild<IB_Coil>();
        //optional if there is no child 
        public void SetReheatCoil(IB_Coil ReheatCoil) => this.ReheatCoil.Set(ReheatCoil);

        //this is the full version
        //public void SetReheatCoil(IB_Coil ReheatCoil)
        //{
        //    this.ReheatCoil.Set(ReheatCoil);
        //}

        public IB_AirTerminalSingleDuctVAVReheat() : base(InitMethod(new Model()))
        {
            //optional if there is no child 
            //Added child with action to Children list, for later automation
            var reheatCoil = new IB_Child(new IB_CoilHeatingWater(), (obj) => this.SetReheatCoil(obj as IB_Coil));
            this.Children.Add(reheatCoil);
            //this.ReheatCoil = new IB_Child<IB_Coil>(new IB_CoilHeatingWater(), this.SetReheatCoil);


            //this.Children.First().GetAction<IB_Coil>()()

        }

        
        

        

        protected override ModelObject InitOpsObj(Model model)
        {
            var newOSObj = base.OnInitOpsObj(InitMethodWithCoil, model).to_AirTerminalSingleDuctVAVReheat().get();
            //var newOSCoil = (HVACComponent)this.ReheatCoil.Get<IB_Coil>().ToOS(model);
            //newOSObj.setReheatCoil(newOSCoil);//TODO: this one to one assign. need to call puppets' ToOS method as well.

            return newOSObj;
        }

        //optional if there is no child 
        //public override ModelObject ToOS(Model model)
        //{
        //    //Maintaining the puppets,
        //    foreach (var item in this.GetPuppets())
        //    {
        //        ((IB_AirTerminalSingleDuctVAVReheat)item).ToOS(model);
        //    }
        //    //return base.ToOS(model); 
        //    //or
        //    return this.InitOpsObj(model); //prefer this as there is no extra logic jump
        //}


        //protected override RelationWithChild<IIB_ModelObject> RelationshipsWithChild => SetReheatCoil;

        //private static Func<IB_AirTerminalSingleDuctVAVReheat> IB_InitFunc2 = () => new IB_AirTerminalSingleDuctVAVReheat();





        //public void SetReheatCoil(IB_Coil ReheatCoil)
        //{
        //    this.ReheatCoil = ReheatCoil;
        //}

        //public override IB_ModelObject Duplicate()
        //{
        //    var newObj = base.Duplicate(); //duplicate self

        //    var newChild = this.ReheatCoil.Duplicate();
        //    var newCoil = this.ReheatCoil.DuplicateChild(); //duplicate child in this class




        //    newObj.Children.First().Inst().GetAction()(newCoil);

        //    newObj.SetRelationships(RelationshipsWithChild, newCoil);
        //    return newObj;
        //}


        //public IB_ModelObject DuplicateAsPuppet()
        //{
        //    //var newObj = (IB_AirTerminalSingleDuctVAVReheat)base.DuplicateIBObj(() => new IB_AirTerminalSingleDuctVAVReheat());
        //    var newObj = this.DuplicateAsPuppet(IB_InitFunc2);
        //    var newCoil = (IB_Coil)this.ReheatCoil.DuplicateAsPuppet();
        //    newObj.SetReheatCoil(newCoil);
        //    return newObj;
        //}

        //public  IB_ModelObject Duplicate2()
        //{
        //    Func<IB_ModelObject> duplicateSelf = ()=> base.DuplicateIBObj(IB_InitFunc);
        //    Func<IB_Coil> duplicateCoil = ()=> this.ReheatCoil.Duplicate() as IB_Coil;
        //    var newObj =  MaintainRelationships(duplicateSelf, duplicateCoil, RelationshipsWithChild);
        //    return newObj;
        //}

        //public IB_ModelObject Duplicate3()
        //{
        //    var newObj = (IB_AirTerminalSingleDuctVAVReheat)base.DuplicateIBObj(IB_InitFunc);
        //    var newCoil = (IB_Coil)this.ReheatCoil.Duplicate();
        //    newObj.SetRelationships(RelationshipsWithChild, newCoil);
        //    return newObj;
        //}

        //public IB_ModelObject MaintainRelationships<T>(Func<IB_ModelObject> hostFunc, Func<T> childFunc, Action<T> doAction) where T: IB_ModelObject
        //{

        //    var host = hostFunc();
        //    var child = childFunc();
        //    host.SetRelationships<T>(doAction, child);

        //    return host;
        //}





        //public override void SetAllTrackingIDs()
        //{
        //    this.ReheatCoil.SetAllTrackingIDs();
        //    base.SetAllTrackingIDs();

        //}
        /// <summary>
        /// This is the public entry point to convert IB object to OpenStudio Object.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

    }
    
}
