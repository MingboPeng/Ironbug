using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveQuartic : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveQuartic();
        private static CurveQuartic InitMethod(Model model)
            => new CurveQuartic(model);
        

        public IB_CurveQuartic():base(InitMethod(new Model()))
        {
        }
        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CurveQuartic().get();
        }
    }

    public sealed class IB_CurveQuartic_DataFieldSet
        : IB_FieldSet<IB_CurveQuartic_DataFieldSet, CurveQuartic>
    {
        private IB_CurveQuartic_DataFieldSet() { }
    }
}
