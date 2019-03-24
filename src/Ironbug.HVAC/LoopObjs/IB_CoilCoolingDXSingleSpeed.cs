using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingDXSingleSpeed : IB_CoilDX
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingDXSingleSpeed();

        private static CoilCoolingDXSingleSpeed NewDefaultOpsObj(Model model) => new CoilCoolingDXSingleSpeed(model);

        public IB_CoilCoolingDXSingleSpeed() : base(NewDefaultOpsObj(new Model()))
        {
            
        }
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_CoilCoolingDXSingleSpeed_FieldSet
        : IB_FieldSet<IB_CoilCoolingDXSingleSpeed_FieldSet, CoilCoolingDXSingleSpeed>
    {
        private IB_CoilCoolingDXSingleSpeed_FieldSet() { }

    }
}
