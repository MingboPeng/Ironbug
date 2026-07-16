using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveQuadraticLinear : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveQuadraticLinear();
        private static CurveQuadraticLinear NewDefaultOpsObj(Model model)
            => new CurveQuadraticLinear(model);
        

        public IB_CurveQuadraticLinear():base(NewDefaultOpsObj)
        {
        }

        public override Curve ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
  
    }

    public sealed class IB_CurveQuadraticLinear_FieldSet
        : IB_FieldSet<IB_CurveQuadraticLinear_FieldSet, CurveQuadraticLinear>
    {
        private IB_CurveQuadraticLinear_FieldSet() { }
    }
}
