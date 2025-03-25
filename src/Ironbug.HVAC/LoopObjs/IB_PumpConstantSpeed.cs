﻿using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_PumpConstantSpeed : IB_Pump
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_PumpConstantSpeed();

        private static PumpConstantSpeed NewDefaultOpsObj(Model model) => new PumpConstantSpeed(model);
        public IB_PumpConstantSpeed():base(NewDefaultOpsObj)
        {
            
        }
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_PumpConstantSpeed_FieldSet 
        : IB_FieldSet<IB_PumpConstantSpeed_FieldSet, PumpConstantSpeed>
    {
        private IB_PumpConstantSpeed_FieldSet() {}


        public IB_Field RatedPumpHead { get; }
            = new IB_BasicField("RatedPumpHead", "PumpHead");

        public IB_Field MotorEfficiency { get; }
            = new IB_BasicField("MotorEfficiency", "Efficiency");

        public IB_Field RatedFlowRate { get; }
            = new IB_BasicField("RatedFlowRate", "FlowRate");

        public IB_Field PumpControlType { get; }
            = new IB_BasicField("PumpControlType", "ControlType");

       

    }
}
