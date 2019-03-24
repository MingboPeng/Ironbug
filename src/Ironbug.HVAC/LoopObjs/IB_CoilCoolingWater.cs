using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingWater : IB_CoilCoolingBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingWater();

        private static CoilCoolingWater NewDefaultOpsObj(Model model) => new CoilCoolingWater(model);
        private IB_ControllerWaterCoil Controller => this.Children.Get<IB_ControllerWaterCoil>();

        public IB_CoilCoolingWater() : base(NewDefaultOpsObj(new Model()))
        {
        }
        public IB_CoilCoolingWater(IB_ControllerWaterCoil Controller) : base(NewDefaultOpsObj(new Model()))
        {
            AddChild(Controller);
        }
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            var obj = ToOS(model);
            var success = obj.addToNode(node);
            if (success && this.Controller != null)
            {
                var optionalCtrl = ((CoilCoolingWater)obj).controllerWaterCoil();
                if (optionalCtrl.is_initialized())
                {
                    optionalCtrl.get().SetCustomAttributes(this.Controller.CustomAttributes);
                }
            }
            return success;
        }
    }
    public sealed class IB_CoilCoolingWater_FieldSet 
        : IB_FieldSet<IB_CoilCoolingWater_FieldSet, CoilCoolingWater>
    {
        private IB_CoilCoolingWater_FieldSet() { }
        
    }

}
