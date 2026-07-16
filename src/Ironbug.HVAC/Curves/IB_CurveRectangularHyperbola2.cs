using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveRectangularHyperbola2 : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveRectangularHyperbola2();
        private static CurveRectangularHyperbola2 NewDefaultOpsObj(Model model)
            => new CurveRectangularHyperbola2(model);
        

        public IB_CurveRectangularHyperbola2():base(NewDefaultOpsObj)
        {
        }

        public override Curve ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
  
    }

    public sealed class IB_CurveRectangularHyperbola2_FieldSet
        : IB_FieldSet<IB_CurveRectangularHyperbola2_FieldSet, CurveRectangularHyperbola2>
    {
        private IB_CurveRectangularHyperbola2_FieldSet() { }
    }
}
