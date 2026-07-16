using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveExponentialDecay: IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveExponentialDecay();
        private static CurveExponentialDecay NewDefaultOpsObj(Model model)
            => new CurveExponentialDecay(model);
        

        public IB_CurveExponentialDecay():base(NewDefaultOpsObj)
        {
        }

        public override Curve ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
  
    }

    public sealed class IB_CurveExponentialDecay_FieldSet
        : IB_FieldSet<IB_CurveExponentialDecay_FieldSet, CurveExponentialDecay>
    {
        private IB_CurveExponentialDecay_FieldSet() { }
    }
}
