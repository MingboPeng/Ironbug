using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveQuadratic : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveQuadratic();
        private static CurveQuadratic NewDefaultOpsObj(Model model)
            => new CurveQuadratic(model);
        

        public IB_CurveQuadratic():base(NewDefaultOpsObj(new Model()))
        {
        }
        public override Curve ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_CurveQuadratic_FieldSet
        : IB_FieldSet<IB_CurveQuadratic_FieldSet, CurveQuadratic>
    {
        private IB_CurveQuadratic_FieldSet() { }

        public IB_Field Coefficient1Constant { get; }
            = new IB_TopField("Coefficient1Constant", "C1");

        public IB_Field Coefficient2x { get; }
            = new IB_TopField("Coefficient2x", "C2");

        public IB_Field Coefficient3xPOW2 { get; }
            = new IB_TopField("Coefficient3xPOW2", "C3");
    }
}
