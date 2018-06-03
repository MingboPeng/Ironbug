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
    }
}
