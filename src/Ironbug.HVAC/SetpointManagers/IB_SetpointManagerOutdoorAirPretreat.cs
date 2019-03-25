using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerOutdoorAirPretreat : IB_SetpointManager
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerOutdoorAirPretreat();

        private static SetpointManagerOutdoorAirPretreat NewDefaultOpsObj(Model model) => new SetpointManagerOutdoorAirPretreat(model);


        public IB_SetpointManagerOutdoorAirPretreat() : base(NewDefaultOpsObj(new Model()))
        { 
        }
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_SetpointManagerOutdoorAirPretreat_FieldSet
        : IB_FieldSet<IB_SetpointManagerOutdoorAirPretreat_FieldSet, SetpointManagerOutdoorAirPretreat>
    {
        private IB_SetpointManagerOutdoorAirPretreat_FieldSet() { }
        
    }
}
