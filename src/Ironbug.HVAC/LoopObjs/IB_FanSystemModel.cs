using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_FanSystemModel : IB_Fan
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_FanSystemModel();
        
        private static FanSystemModel NewDefaultOpsObj(Model model) => new FanSystemModel(model);

        public IB_FanSystemModel():base(NewDefaultOpsObj(new Model()))
        {
        }
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        
        
    }



    public sealed class IB_FanSystemModel_FieldSet
        : IB_FieldSet<IB_FanSystemModel_FieldSet, FanSystemModel>
    {
        private IB_FanSystemModel_FieldSet() {}
        
        
    }
}
