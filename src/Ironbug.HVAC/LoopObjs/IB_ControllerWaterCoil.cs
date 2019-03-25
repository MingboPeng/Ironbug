using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ControllerWaterCoil : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ControllerWaterCoil();

        private static ControllerWaterCoil NewDefaultOpsObj(Model model) => new ControllerWaterCoil(model);
        public IB_ControllerWaterCoil() : base(NewDefaultOpsObj(new Model()))
        {
        }
        
        public ModelObject ToOS(Model model)
        {
            var newObj = this.OnNewOpsObj(NewDefaultOpsObj, model);
            return newObj;
        }

    }

    public sealed class IB_ControllerWaterCoil_FieldSet
        : IB_FieldSet<IB_ControllerWaterCoil_FieldSet, ControllerWaterCoil>
    {
        private IB_ControllerWaterCoil_FieldSet() {}
        
        
    }
}
