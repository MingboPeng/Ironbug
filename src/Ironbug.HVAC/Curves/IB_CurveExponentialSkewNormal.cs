using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveExponentialSkewNormal : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveExponentialSkewNormal();
        private static CurveExponentialSkewNormal NewDefaultOpsObj(Model model)
            => new CurveExponentialSkewNormal(model);
        

        public IB_CurveExponentialSkewNormal():base(NewDefaultOpsObj)
        {
        }

        public override Curve ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
  
    }

    public sealed class IB_CurveExponentialSkewNormal_FieldSet
        : IB_FieldSet<IB_CurveExponentialSkewNormal_FieldSet, CurveExponentialSkewNormal>
    {
        private IB_CurveExponentialSkewNormal_FieldSet() { }
    }
}
