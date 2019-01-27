using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingDXMultiSpeed : IB_CoilDX
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingDXMultiSpeed();

        private static CoilHeatingDXMultiSpeed NewDefaultOpsObj(Model model) => new CoilHeatingDXMultiSpeed(model);

        public IB_CoilHeatingDXMultiSpeed() : base(NewDefaultOpsObj(new Model()))
        {
            
        }
        

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }

    }

    public sealed class IB_CoilHeatingDXMultiSpeed_DataFieldSet
        : IB_FieldSet<IB_CoilHeatingDXMultiSpeed_DataFieldSet, CoilHeatingDXMultiSpeed>
    {
        private IB_CoilHeatingDXMultiSpeed_DataFieldSet() { }

    }
}
