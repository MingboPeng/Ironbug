using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveSigmoid : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveSigmoid();
        private static CurveSigmoid NewDefaultOpsObj(Model model)
            => new CurveSigmoid(model);
        

        public IB_CurveSigmoid():base(NewDefaultOpsObj(new Model()))
        {
        }
        protected override ModelObject NewOpsObj(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model).to_CurveSigmoid().get();
        }
    }

    public sealed class IB_CurveSigmoid_DataFieldSet
        : IB_FieldSet<IB_CurveSigmoid_DataFieldSet, CurveSigmoid>
    {
        private IB_CurveSigmoid_DataFieldSet() { }

        public IB_Field Coefficient1C1 { get; }
            = new IB_TopField("Coefficient1C1", "C1");

        public IB_Field Coefficient2C2 { get; }
            = new IB_TopField("Coefficient2C2", "C2");

        public IB_Field Coefficient3C3{ get; }
            = new IB_TopField("Coefficient3C3", "C3");

        public IB_Field Coefficient4C4 { get; }
            = new IB_TopField("Coefficient4C4", "C4");

        public IB_Field Coefficient5C5 { get; }
            = new IB_TopField("Coefficient5C5", "C5");
    }
}
