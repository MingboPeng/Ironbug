using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctConstantVolumeCooledBeam : IB_AirTerminal
    {
        //this is for self duplication and duplication as Puppet
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctVAVReheat();
        //this is for OpenStudio object initialization
        private static AirTerminalSingleDuctConstantVolumeCooledBeam InitMethod(Model model) =>
            new AirTerminalSingleDuctConstantVolumeCooledBeam(model, model.alwaysOnDiscreteSchedule(), new CoilHeatingWater(model));
        
        //Associated with child object 
        //optional if there is no child 
        private IB_Child CoolingCoil => this.Children.GetChild<IB_CoilCoolingCooledBeam>();
        //optional if there is no child 
        public void SetCoolingCoil(IB_CoilCoolingCooledBeam coil) => this.CoolingCoil.Set(coil);
        

        public IB_AirTerminalSingleDuctConstantVolumeCooledBeam() : base(InitMethod(new Model()))
        {
            //optional if there is no child 
            //Added child with action to Children list, for later automation
            var coil = new IB_Child(new IB_CoilHeatingWater(), (obj) => this.SetCoolingCoil(obj as IB_CoilCoolingCooledBeam));
            this.Children.Add(coil);

        }

        

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethodWithCoil, model).to_AirTerminalSingleDuctConstantVolumeCooledBeam().get();

            //Local Method
            AirTerminalSingleDuctConstantVolumeCooledBeam InitMethodWithCoil(Model md) =>
                new AirTerminalSingleDuctConstantVolumeCooledBeam(md, md.alwaysOnDiscreteSchedule(), (HVACComponent)this.CoolingCoil.To<IB_CoilCoolingCooledBeam>().ToOS(md));
        }
        

    }

    public sealed class IB_AirTerminalSingleDuctConstantVolumeCooledBeam_DataFieldSet
        : IB_DataFieldSet<IB_AirTerminalSingleDuctConstantVolumeCooledBeam_DataFieldSet, AirTerminalSingleDuctConstantVolumeCooledBeam>
    {
        private IB_AirTerminalSingleDuctConstantVolumeCooledBeam_DataFieldSet() { }

    }

}
