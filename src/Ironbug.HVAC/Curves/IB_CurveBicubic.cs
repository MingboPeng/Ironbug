using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC.Curves
{
    public class IB_CurveBicubic: IB_Curve
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CurveBicubic();
        private static CurveBicubic InitMethod(Model model)
            => new CurveBicubic(model);
        

        public IB_CurveBicubic():base(InitMethod(new Model()))
        {
        }
        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CurveBicubic().get();
        }
    }

    public sealed class IB_CurveBicubic_DataFieldSet
        : IB_FieldSet<IB_CurveBicubic_DataFieldSet, CurveBicubic>
    {
        private IB_CurveBicubic_DataFieldSet() { }
    }
}
