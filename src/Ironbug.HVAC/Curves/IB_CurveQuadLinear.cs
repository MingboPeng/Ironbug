using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveQuadLinear: IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveQuadLinear();
        private static CurveQuadLinear NewDefaultOpsObj(Model model)
            => new CurveQuadLinear(model);
        

        public IB_CurveQuadLinear():base(NewDefaultOpsObj)
        {
        }

        public override Curve ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
  
    }

    public sealed class IB_CurveQuadLinear_FieldSet
        : IB_FieldSet<IB_CurveQuadLinear_FieldSet, CurveQuadLinear>
    {
        private IB_CurveQuadLinear_FieldSet() { }
    }
}
