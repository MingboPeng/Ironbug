﻿using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctConstantVolumeFourPipeBeam : IB_AirTerminal
    {
        //this is for self duplication and duplication as Puppet
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctConstantVolumeFourPipeBeam();
        //this is for OpenStudio object initialization
        private static AirTerminalSingleDuctConstantVolumeFourPipeBeam NewDefaultOpsObj(Model model) =>
            new AirTerminalSingleDuctConstantVolumeFourPipeBeam(model);
        
        //Associated with child object 
        //optional if there is no child 
        private IB_CoilCoolingFourPipeBeam CoolingCoil => this.GetChild<IB_CoilCoolingFourPipeBeam>();
        private IB_CoilHeatingFourPipeBeam HeatingCoil => this.GetChild<IB_CoilHeatingFourPipeBeam>();
        //optional if there is no child 
        public void SetCoolingCoil(IB_CoilCoolingFourPipeBeam coil) => this.SetChild(coil);
        public void SetHeatingCoil(IB_CoilHeatingFourPipeBeam coil) => this.SetChild(coil);

        [JsonConstructor]
        private IB_AirTerminalSingleDuctConstantVolumeFourPipeBeam(bool forDeserialization) : base(null)
        {
        }
        public IB_AirTerminalSingleDuctConstantVolumeFourPipeBeam() : base(NewDefaultOpsObj)
        {
            //optional if there is no child 
            //Added child with action to Children list, for later automation
            this.AddChild(new IB_CoilCoolingFourPipeBeam());
            this.AddChild(new IB_CoilHeatingFourPipeBeam());

        }



        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(InitMethodWithCoil, model);
            
            AirTerminalSingleDuctConstantVolumeFourPipeBeam InitMethodWithCoil(Model md) =>
                new AirTerminalSingleDuctConstantVolumeFourPipeBeam(md, this.CoolingCoil.ToOS(md), this.HeatingCoil.ToOS(md));
        }
        

    }

    public sealed class IB_AirTerminalSingleDuctConstantVolumeFourPipeBeam_FieldSet
        : IB_FieldSet<IB_AirTerminalSingleDuctConstantVolumeFourPipeBeam_FieldSet>
    {
        private IB_AirTerminalSingleDuctConstantVolumeFourPipeBeam_FieldSet() { }

    }

}
