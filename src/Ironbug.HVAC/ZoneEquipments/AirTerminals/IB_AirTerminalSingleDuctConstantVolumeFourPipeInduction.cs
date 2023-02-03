﻿using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctConstantVolumeFourPipeInduction : IB_AirTerminal
    {
        //this is for self duplication and duplication as Puppet
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctConstantVolumeFourPipeInduction();
        //this is for OpenStudio object initialization
        private static AirTerminalSingleDuctConstantVolumeFourPipeInduction NewDefaultOpsObj(Model model) =>
            new AirTerminalSingleDuctConstantVolumeFourPipeInduction(model, new CoilHeatingWater(model));
        
        //Associated with child object 
        //optional if there is no child 
        private IB_CoilCoolingWater CoolingCoil => this.GetChild<IB_CoilCoolingWater>();
        private IB_CoilHeatingWater HeatingCoil => this.GetChild<IB_CoilHeatingWater>();
        //optional if there is no child 
        public void SetCoolingCoil(IB_CoilCoolingWater coil)
        {
            if (this.CoolingCoil == null)
            {
                this.AddChild(coil);
            }
            else
            {
                this.SetChild(coil);
            }
            

        }
        public void SetHeatingCoil(IB_CoilHeatingWater coil) => this.SetChild(coil);

        [JsonConstructor]
        private IB_AirTerminalSingleDuctConstantVolumeFourPipeInduction(bool forDeserialization) : base(null)
        {
        }
        public IB_AirTerminalSingleDuctConstantVolumeFourPipeInduction() : base(NewDefaultOpsObj(new Model()))
        {
            //optional if there is no child 
            //Added child with action to Children list, for later automation
            //this.AddChild(new IB_CoilCoolingWater());
            this.AddChild(new IB_CoilHeatingWater());

        }



        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(InitMethodWithCoil, model);
            if (this.CoolingCoil != null)
            {
                obj.setCoolingCoil(this.CoolingCoil.ToOS(model));
            }
           

            return obj;
            
            AirTerminalSingleDuctConstantVolumeFourPipeInduction InitMethodWithCoil(Model md) =>
                new AirTerminalSingleDuctConstantVolumeFourPipeInduction(md, this.HeatingCoil.ToOS(md));
        }
        

    }

    public sealed class IB_AirTerminalSingleDuctConstantVolumeFourPipeInduction_FieldSet
        : IB_FieldSet<IB_AirTerminalSingleDuctConstantVolumeFourPipeInduction_FieldSet>
    {
        private IB_AirTerminalSingleDuctConstantVolumeFourPipeInduction_FieldSet() { }

    }

}
