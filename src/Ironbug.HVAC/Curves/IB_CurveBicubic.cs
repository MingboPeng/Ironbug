using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveBicubic: IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveBicubic();
        private static CurveBicubic NewDefaultOpsObj(Model model)
            => new CurveBicubic(model);
        

        public IB_CurveBicubic():base(NewDefaultOpsObj(new Model()))
        {
        }

        public override Curve ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_CurveBicubic_DataFieldSet
        : IB_FieldSet<IB_CurveBicubic_DataFieldSet, CurveBicubic>
    {
        private IB_CurveBicubic_DataFieldSet() { }
        public IB_Field Coefficient1Constant { get; }
            = new IB_TopField("Coefficient1Constant", "C1");

        public IB_Field Coefficient2x { get; }
            = new IB_TopField("Coefficient2x", "C2");

        public IB_Field Coefficient3xPOW2 { get; }
            = new IB_TopField("Coefficient3xPOW2", "C3");

        public IB_Field Coefficient4y { get; }
            = new IB_TopField("Coefficient4y", "C4");

        public IB_Field Coefficient5yPOW2 { get; }
            = new IB_TopField("Coefficient5yPOW2", "C5");

        public IB_Field Coefficient6xTIMESY { get; }
            = new IB_TopField("Coefficient6xTIMESY", "C6");

        public IB_Field Coefficient7xPOW3 { get; }
            = new IB_TopField("Coefficient7xPOW3", "C7");

        public IB_Field Coefficient8yPOW3 { get; }
            = new IB_TopField("Coefficient8yPOW3", "C8");

        public IB_Field Coefficient9xPOW2TIMESY { get; }
            = new IB_TopField("Coefficient9xPOW2TIMESY", "C9");

        public IB_Field Coefficient10xTIMESYPOW2 { get; }
            = new IB_TopField("Coefficient10xTIMESYPOW2", "C10");
    }
}
