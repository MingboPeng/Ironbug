using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ControllerMechanicalVentilation : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ControllerMechanicalVentilation();

        private static ControllerMechanicalVentilation NewDefaultOpsObj(Model model) => new ControllerMechanicalVentilation(model);
        public IB_ControllerMechanicalVentilation() : base(NewDefaultOpsObj)
        {
        }
        
        public ModelObject ToOS(Model model)
        {
            var newObj = this.OnNewOpsObj(NewDefaultOpsObj, model);
            return newObj;
        }

    }

    public sealed class IB_ControllerMechanicalVentilation_FieldSet
        : IB_FieldSet<IB_ControllerMechanicalVentilation_FieldSet, ControllerMechanicalVentilation>
    {
        private IB_ControllerMechanicalVentilation_FieldSet() {}
        
        public IB_Field DemandControlledVentilation { get; }
            = new IB_BasicField("DemandControlledVentilation", "DCV");
        
        
    }
}
