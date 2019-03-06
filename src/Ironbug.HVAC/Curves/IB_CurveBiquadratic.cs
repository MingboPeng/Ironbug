using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveBiquadratic : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveBiquadratic();
        private static CurveBiquadratic NewDefaultOpsObj(Model model)
            => new CurveBiquadratic(model);
        

        public IB_CurveBiquadratic():base(NewDefaultOpsObj(new Model())) { }

        public override Curve ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }

        //protected override ModelObject NewOpsObj(Model model)
        //{
        //    return base.OnNewOpsObj(NewDefaultOpsObj, model).to_CurveBiquadratic().get();
        //}
    }

    public sealed class IB_CurveBiquadratic_DataFieldSet
        : IB_FieldSet<IB_CurveBiquadratic_DataFieldSet, CurveBiquadratic>
    {
        private IB_CurveBiquadratic_DataFieldSet() { }

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
    }
}
