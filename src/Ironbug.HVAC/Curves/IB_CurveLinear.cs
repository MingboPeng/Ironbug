using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveLinear : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveLinear();
        private static CurveLinear NewDefaultOpsObj(Model model)
            => new CurveLinear(model);
        

        public IB_CurveLinear():base(NewDefaultOpsObj(new Model()))
        {
        }
        protected override ModelObject NewOpsObj(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model).to_CurveLinear().get();
        }
    }

    public sealed class IB_CurveLinear_DataFieldSet
        : IB_FieldSet<IB_CurveLinear_DataFieldSet, CurveLinear>
    {
        private IB_CurveLinear_DataFieldSet() { }
        public IB_Field Coefficient1Constant { get; }
            = new IB_TopField("Coefficient1Constant", "C1");

        public IB_Field Coefficient2x { get; }
            = new IB_TopField("Coefficient2x", "C2");
    }
}
