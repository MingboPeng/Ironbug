using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_FanOnOff : IB_Fan
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_FanOnOff();
        
        private static FanOnOff NewDefaultOpsObj(Model model) => new FanOnOff(model);

        public IB_FanOnOff():base(NewDefaultOpsObj(new Model()))
        {
        }
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        
        
    }



    public sealed class IB_FanOnOff_FieldSet
        : IB_FieldSet<IB_FanOnOff_FieldSet, FanOnOff>
    {
        private IB_FanOnOff_FieldSet() {}
        
        
    }
}
