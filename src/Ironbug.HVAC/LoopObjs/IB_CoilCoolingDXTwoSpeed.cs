﻿using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingDXTwoSpeed : IB_CoilDX
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingDXTwoSpeed();

        private static CoilCoolingDXTwoSpeed NewDefaultOpsObj(Model model) => new CoilCoolingDXTwoSpeed(model);

        public IB_CoilCoolingDXTwoSpeed() : base(NewDefaultOpsObj)
        {
        }
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_CoilCoolingDXTwoSpeed_FieldSet
        : IB_FieldSet<IB_CoilCoolingDXTwoSpeed_FieldSet, CoilCoolingDXTwoSpeed>
    {
        private IB_CoilCoolingDXTwoSpeed_FieldSet() { }

    }
}
