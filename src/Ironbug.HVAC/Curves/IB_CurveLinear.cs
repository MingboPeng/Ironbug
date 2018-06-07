using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveLinear : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveLinear();
        private static CurveLinear InitMethod(Model model)
            => new CurveLinear(model);
        

        public IB_CurveLinear():base(InitMethod(new Model()))
        {
        }
        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CurveLinear().get();
        }
    }

    public sealed class IB_CurveLinear_DataFieldSet
        : IB_FieldSet<IB_CurveLinear_DataFieldSet, CurveLinear>
    {
        private IB_CurveLinear_DataFieldSet() { }
    }
}
