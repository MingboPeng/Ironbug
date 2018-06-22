using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ControllerMechanicalVentilation : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ControllerMechanicalVentilation();

        private static ControllerMechanicalVentilation InitMethod(Model model) => new ControllerMechanicalVentilation(model);
        public IB_ControllerMechanicalVentilation() : base(InitMethod(new Model()))
        {
        }

        public override IB_ModelObject Duplicate()
        {
            return this.DuplicateIBObj(() => new IB_ControllerMechanicalVentilation());
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            var newObj = this.OnInitOpsObj(InitMethod, model).to_ControllerMechanicalVentilation().get();
            return newObj;
        }

        public ModelObject ToOS(Model model)
        {
            return this.InitOpsObj(model);
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
