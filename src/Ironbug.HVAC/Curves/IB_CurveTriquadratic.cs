﻿using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveTriquadratic : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveTriquadratic();
        private static CurveTriquadratic NewDefaultOpsObj(Model model)
            => new CurveTriquadratic(model);
        

        public IB_CurveTriquadratic():base(NewDefaultOpsObj(new Model()))
        {
        }
        public override Curve ToOS()
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, new Model());
        }
    }

    public sealed class IB_CurveTriquadratic_DataFieldSet
        : IB_FieldSet<IB_CurveTriquadratic_DataFieldSet, CurveTriquadratic>
    {
        private IB_CurveTriquadratic_DataFieldSet() { }
        
    }
}
