﻿using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctConstantVolumeCooledBeam : IB_AirTerminal
    {
        //this is for self duplication and duplication as Puppet
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctConstantVolumeCooledBeam();
        //this is for OpenStudio object initialization
        private static AirTerminalSingleDuctConstantVolumeCooledBeam NewDefaultOpsObj(Model model) =>
            new AirTerminalSingleDuctConstantVolumeCooledBeam(model, model.alwaysOnDiscreteSchedule(), new CoilHeatingWater(model));
        
        //Associated with child object 
        //optional if there is no child 
        private IB_CoilCoolingCooledBeam CoolingCoil => this.GetChild<IB_CoilCoolingCooledBeam>();
        //optional if there is no child 
        public void SetCoolingCoil(IB_CoilCoolingCooledBeam coil) => this.SetChild(coil);


        [JsonConstructor]
        private IB_AirTerminalSingleDuctConstantVolumeCooledBeam(bool forDeserialization) : base(null)
        {
        }

        public IB_AirTerminalSingleDuctConstantVolumeCooledBeam() : base(NewDefaultOpsObj(new Model()))
        {
            //optional if there is no child 
            //Added child with action to Children list, for later automation
            this.AddChild(new IB_CoilCoolingCooledBeam());

        }



        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(InitMethodWithCoil, model);
            
            AirTerminalSingleDuctConstantVolumeCooledBeam InitMethodWithCoil(Model md) =>
                new AirTerminalSingleDuctConstantVolumeCooledBeam(md, md.alwaysOnDiscreteSchedule(), this.CoolingCoil.ToOS(md));
        }
        

    }

    public sealed class IB_AirTerminalSingleDuctConstantVolumeCooledBeam_FieldSet
        : IB_FieldSet<IB_AirTerminalSingleDuctConstantVolumeCooledBeam_FieldSet, AirTerminalSingleDuctConstantVolumeCooledBeam>
    {
        private IB_AirTerminalSingleDuctConstantVolumeCooledBeam_FieldSet() { }

    }

}
