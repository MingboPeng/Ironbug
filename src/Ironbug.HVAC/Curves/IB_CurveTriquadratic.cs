using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveTriquadratic : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveTriquadratic();
        private static CurveTriquadratic InitMethod(Model model)
            => new CurveTriquadratic(model);
        

        public IB_CurveTriquadratic():base(InitMethod(new Model()))
        {
        }
        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CurveTriquadratic().get();
        }
    }

    public sealed class IB_CurveTriquadratic_DataFieldSet
        : IB_FieldSet<IB_CurveTriquadratic_DataFieldSet, CurveSigmoid>
    {
        private IB_CurveTriquadratic_DataFieldSet() { }
    }
}
