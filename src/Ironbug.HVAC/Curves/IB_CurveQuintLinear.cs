using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveQuintLinear: IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveQuintLinear();
        private static CurveQuintLinear NewDefaultOpsObj(Model model)
            => new CurveQuintLinear(model);
        

        public IB_CurveQuintLinear():base(NewDefaultOpsObj)
        {
        }

        public override Curve ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
  
    }

    public sealed class IB_CurveQuintLinear_FieldSet
        : IB_FieldSet<IB_CurveQuintLinear_FieldSet, CurveQuintLinear>
    {
        private IB_CurveQuintLinear_FieldSet() { }
    }
}
