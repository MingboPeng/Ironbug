using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveDoubleExponentialDecay : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveDoubleExponentialDecay();
        private static CurveDoubleExponentialDecay NewDefaultOpsObj(Model model)
            => new CurveDoubleExponentialDecay(model);
        

        public IB_CurveDoubleExponentialDecay():base(NewDefaultOpsObj)
        {
        }

        public override Curve ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
  
    }

    public sealed class IB_CurveDoubleExponentialDecay_FieldSet
        : IB_FieldSet<IB_CurveDoubleExponentialDecay_FieldSet, CurveDoubleExponentialDecay>
    {
        private IB_CurveDoubleExponentialDecay_FieldSet() { }
    }
}
