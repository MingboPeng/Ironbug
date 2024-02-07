using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerMixedAir : IB_SetpointManager
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerMixedAir();

        private static SetpointManagerMixedAir NewDefaultOpsObj(Model model) => new SetpointManagerMixedAir(model);
        public IB_SetpointManagerMixedAir() : base(NewDefaultOpsObj(new Model()))
        {
            
        } 

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_SetpointManagerMixedAir_FieldSet
        : IB_FieldSet<IB_SetpointManagerMixedAir_FieldSet, SetpointManagerMixedAir>
    {
        private IB_SetpointManagerMixedAir_FieldSet() { }

    }


}
