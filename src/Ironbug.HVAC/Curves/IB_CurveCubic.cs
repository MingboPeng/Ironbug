using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveCubic : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveCubic();
        private static CurveCubic InitMethod(Model model)
            => new CurveCubic(model);
        

        public IB_CurveCubic():base(InitMethod(new Model()))
        {
        }
        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CurveCubic().get();
        }
    }

    public sealed class IB_CurveCubic_DataFieldSet
        : IB_FieldSet<IB_CurveCubic_DataFieldSet, CurveCubic>
    {
        private IB_CurveCubic_DataFieldSet() { }
    }
}
