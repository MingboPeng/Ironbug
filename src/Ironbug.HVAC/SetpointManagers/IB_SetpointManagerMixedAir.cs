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
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            // this will be executed after all loops (nodes) are saved
            Func<bool> func = () =>
            {
                var airloopO = obj.airLoopHVAC();
                if (airloopO == null || !airloopO.is_initialized())
                    throw new ArgumentException("Failed to find AirLoopHVAC for SetpointManagerMixedAir");

                var lp = airloopO.get();
                SetpointManagerMixedAir.updateFanInletOutletNodes(lp);
                return true;

            };

            IB_Utility.AddDelayFunc(func);

            return obj;
        }
    }

    public sealed class IB_SetpointManagerMixedAir_FieldSet
        : IB_FieldSet<IB_SetpointManagerMixedAir_FieldSet, SetpointManagerMixedAir>
    {
        private IB_SetpointManagerMixedAir_FieldSet() { }

    }


}
