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
        public IB_Field Coefficient1Constant { get; }
            = new IB_TopField("Coefficient1Constant", "Const");

        public IB_Field Coefficient2x { get; }
            = new IB_TopField("Coefficient2x", "x");

        public IB_Field Coefficient3xPOW2 { get; }
            = new IB_TopField("Coefficient3xPOW2", "xPOW2");

        public IB_Field Coefficient4xPOW3 { get; }
            = new IB_TopField("Coefficient4xPOW3", "xPOW3");

        public IB_Field Coefficient5xPOW4 { get; }
            = new IB_TopField("Coefficient5xPOW4", "xPOW4");

    }
}
