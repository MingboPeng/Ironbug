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
        
        //Associated child object 
        //optional if there is no child 
        private IB_Child ReheatCoil => this.Children.GetChild<IB_CoilBasic>();
        //optional if there is no child 
        public void SetReheatCoil(IB_CoilBasic ReheatCoil) => this.ReheatCoil.Set(ReheatCoil);
        

        public IB_AirTerminalSingleDuctVAVReheat() : base(InitMethod(new Model()))
        {
            //optional if there is no child 
            //Added child with action to Children list, for later automation
            var reheatCoil = new IB_Child(new IB_CoilHeatingWater(), (obj) => this.SetReheatCoil(obj as IB_CoilBasic));

        }

        

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethodWithCoil, model).to_AirTerminalSingleDuctVAVReheat().get();
            
            //Local Method
            AirTerminalSingleDuctVAVReheat InitMethodWithCoil(Model md) =>
                new AirTerminalSingleDuctVAVReheat(md, md.alwaysOnDiscreteSchedule(), (HVACComponent)this.ReheatCoil.To<IB_CoilBasic>().ToOS(md));
        }
        

    }

    public sealed class IB_AirTerminalSingleDuctVAVReheat_DataFieldSet
        : IB_DataFieldSet<IB_AirTerminalSingleDuctVAVReheat_DataFieldSet, AirTerminalSingleDuctVAVReheat>
    {
        private IB_AirTerminalSingleDuctVAVReheat_DataFieldSet() { }

    }

}
