using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveRectangularHyperbola1 : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveRectangularHyperbola1();
        private static CurveRectangularHyperbola1 NewDefaultOpsObj(Model model)
            => new CurveRectangularHyperbola1(model);
        

        public IB_CurveRectangularHyperbola1():base(NewDefaultOpsObj)
        {
        }

        public override Curve ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
  
    }

    public sealed class IB_CurveRectangularHyperbola1_FieldSet
        : IB_FieldSet<IB_CurveRectangularHyperbola1_FieldSet, CurveRectangularHyperbola1>
    {
        private IB_CurveRectangularHyperbola1_FieldSet() { }
    }
}
