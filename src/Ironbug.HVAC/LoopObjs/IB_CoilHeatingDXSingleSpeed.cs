﻿using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingDXSingleSpeed: IB_CoilDX
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingDXSingleSpeed();
        private static CoilHeatingDXSingleSpeed NewDefaultOpsObj(Model model) => new CoilHeatingDXSingleSpeed(model);

        public IB_CoilHeatingDXSingleSpeed() : base(NewDefaultOpsObj)
        {

        }
        


        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }

    }

    public sealed class IB_CoilHeatingDXSingleSpeed_FieldSet
        : IB_FieldSet<IB_CoilHeatingDXSingleSpeed_FieldSet, CoilHeatingDXSingleSpeed>
    {
        private IB_CoilHeatingDXSingleSpeed_FieldSet() { }

    }
}
