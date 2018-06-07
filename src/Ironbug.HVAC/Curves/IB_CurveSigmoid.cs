using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveSigmoid : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveSigmoid();
        private static CurveSigmoid InitMethod(Model model)
            => new CurveSigmoid(model);
        

        public IB_CurveSigmoid():base(InitMethod(new Model()))
        {
        }
        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CurveSigmoid().get();
        }
    }

    public sealed class IB_CurveSigmoid_DataFieldSet
        : IB_FieldSet<IB_CurveSigmoid_DataFieldSet, CurveSigmoid>
    {
        private IB_CurveSigmoid_DataFieldSet() { }
    }
}
