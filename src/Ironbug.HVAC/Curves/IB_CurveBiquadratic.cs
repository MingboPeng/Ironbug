using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveBiquadratic : IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveBiquadratic();
        private static CurveBiquadratic InitMethod(Model model)
            => new CurveBiquadratic(model);
        

        public IB_CurveBiquadratic():base(InitMethod(new Model())) { }
        
        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CurveBiquadratic().get();
        }
    }

    public sealed class IB_CurveBiquadratic_DataFieldSet
        : IB_FieldSet<IB_CurveBiquadratic_DataFieldSet, CurveBiquadratic>
    {
        private IB_CurveBiquadratic_DataFieldSet() { }

        public IB_Field Coefficient1Constant { get; }
            = new IB_TopField("Coefficient1Constant", "Const");

        public IB_Field Coefficient2x { get; }
            = new IB_TopField("Coefficient2x", "x");

        public IB_Field Coefficient3xPOW2 { get; }
            = new IB_TopField("Coefficient3xPOW2", "xPOW2");

        public IB_Field Coefficient4y { get; }
            = new IB_TopField("Coefficient4y", "y");

        public IB_Field Coefficient5yPOW2 { get; }
            = new IB_TopField("Coefficient5yPOW2", "yPOW2");

        public IB_Field Coefficient6xTIMESY { get; }
            = new IB_TopField("Coefficient6xTIMESY", "xTIMESY");
    }
}
