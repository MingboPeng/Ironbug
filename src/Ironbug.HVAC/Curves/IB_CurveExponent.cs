﻿using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveExponent : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveExponent();
        private static CurveExponent NewDefaultOpsObj(Model model)
            => new CurveExponent(model);
        

        public IB_CurveExponent():base(NewDefaultOpsObj(new Model()))
        {
        }
        public override Curve ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_CurveExponent_FieldSet
        : IB_FieldSet<IB_CurveExponent_FieldSet, CurveExponent>
    {
        private IB_CurveExponent_FieldSet() { }
        public IB_Field Coefficient1Constant { get; }
            = new IB_TopField("Coefficient1Constant", "C1");

        public IB_Field Coefficient2Constant { get; }
            = new IB_TopField("Coefficient2Constant", "C2");

        public IB_Field Coefficient3Constant { get; }
            = new IB_TopField("Coefficient3Constant", "C3");
    }
}
