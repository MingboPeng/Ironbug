using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ControllerMechanicalVentilation : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ControllerMechanicalVentilation();

        private static ControllerMechanicalVentilation NewDefaultOpsObj(Model model) => new ControllerMechanicalVentilation(model);
        public IB_ControllerMechanicalVentilation() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public override IB_ModelObject Duplicate()
        {
            return this.DuplicateIBObj(() => new IB_ControllerMechanicalVentilation());
        }

        protected override ModelObject NewOpsObj(Model model)
        {
            var newObj = this.OnNewOpsObj(NewDefaultOpsObj, model).to_ControllerMechanicalVentilation().get();
            return newObj;
        }

        public ModelObject ToOS(Model model)
        {
            return this.NewOpsObj(model);
        }

    }

    public sealed class IB_ControllerMechanicalVentilation_DataFieldSet
        : IB_FieldSet<IB_ControllerMechanicalVentilation_DataFieldSet, ControllerMechanicalVentilation>
    {
        private IB_ControllerMechanicalVentilation_DataFieldSet() {}
        
        public IB_Field DemandControlledVentilation { get; }
            = new IB_BasicField("DemandControlledVentilation", "DCV");
        
        
    }
}
