﻿using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveQuartic : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveQuartic();
        private static CurveQuartic NewDefaultOpsObj(Model model)
            => new CurveQuartic(model);
        

        public IB_CurveQuartic():base(NewDefaultOpsObj(new Model()))
        {
        }
        public override Curve ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_CurveQuartic_FieldSet
        : IB_FieldSet<IB_CurveQuartic_FieldSet, CurveQuartic>
    {
        private IB_CurveQuartic_FieldSet() { }
        public IB_Field Coefficient1Constant { get; }
            = new IB_TopField("Coefficient1Constant", "C1");

        public IB_Field Coefficient2x { get; }
            = new IB_TopField("Coefficient2x", "C2");

        public IB_Field Coefficient3xPOW2 { get; }
            = new IB_TopField("Coefficient3xPOW2", "C3");

        public IB_Field Coefficient4xPOW3 { get; }
            = new IB_TopField("Coefficient4xPOW3", "xPOW3");

        public IB_Field Coefficient5xPOW4 { get; }
            = new IB_TopField("Coefficient5xPOW4", "xPOW4");

    }
}
