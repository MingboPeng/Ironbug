using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingDXMultiSpeed : IB_CoilDX
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingDXMultiSpeed();

        private static CoilCoolingDXMultiSpeed NewDefaultOpsObj(Model model) => new CoilCoolingDXMultiSpeed(model);

        public IB_CoilCoolingDXMultiSpeed() : base(NewDefaultOpsObj(new Model()))
        {
            
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }

    }

    public sealed class IB_CoilCoolingDXMultiSpeed_DataFieldSet
        : IB_FieldSet<IB_CoilCoolingDXMultiSpeed_DataFieldSet, CoilCoolingDXMultiSpeed>
    {
        private IB_CoilCoolingDXMultiSpeed_DataFieldSet() { }

    }
}
