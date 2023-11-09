using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerFollowGroundTemperature : IB_SetpointManager
    {
   
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerFollowGroundTemperature();

        private static SetpointManagerFollowGroundTemperature NewDefaultOpsObj(Model model) => new SetpointManagerFollowGroundTemperature(model);


        public IB_SetpointManagerFollowGroundTemperature() : base(NewDefaultOpsObj(new Model()))
        { 
        }


        public override HVACComponent ToOS(Model model)
        {
            var obj =  base.OnNewOpsObj(NewDefaultOpsObj, model);
            return obj;
        }
    }

    public sealed class IB_SetpointManagerFollowGroundTemperature_FieldSet
        : IB_FieldSet<IB_SetpointManagerFollowGroundTemperature_FieldSet, SetpointManagerFollowGroundTemperature>
    {
        private IB_SetpointManagerFollowGroundTemperature_FieldSet() { }

    }
}
