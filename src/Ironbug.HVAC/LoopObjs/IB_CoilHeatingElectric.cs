using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingElectric : IB_CoilHeatingBasic
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingElectric();

        private static CoilHeatingElectric NewDefaultOpsObj(Model model) => new CoilHeatingElectric(model);

        public IB_CoilHeatingElectric() : base(NewDefaultOpsObj(new Model()))
        {
        }
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_CoilHeatingElectric_FieldSet 
        : IB_FieldSet<IB_CoilHeatingElectric_FieldSet , CoilHeatingElectric>
    {
        private IB_CoilHeatingElectric_FieldSet() {}
    }
}