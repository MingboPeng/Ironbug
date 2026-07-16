using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveFunctionalPressureDrop : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveFunctionalPressureDrop();
        private static CurveFunctionalPressureDrop NewDefaultOpsObj(Model model)
            => new CurveFunctionalPressureDrop(model);
        

        public IB_CurveFunctionalPressureDrop():base(NewDefaultOpsObj)
        {
        }

        public override Curve ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
  
    }

    public sealed class IB_CurveFunctionalPressureDrop_FieldSet
        : IB_FieldSet<IB_CurveFunctionalPressureDrop_FieldSet, CurveFunctionalPressureDrop>
    {
        private IB_CurveFunctionalPressureDrop_FieldSet() { }
    }
}
