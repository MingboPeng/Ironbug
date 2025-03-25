using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_FluidCoolerTwoSpeed : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_FluidCoolerTwoSpeed();

        private static FluidCoolerTwoSpeed NewDefaultOpsObj(Model model) => new FluidCoolerTwoSpeed(model);
        public IB_FluidCoolerTwoSpeed() : base(NewDefaultOpsObj)
        {
        }
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_FluidCoolerTwoSpeed_FieldSet
        : IB_FieldSet<IB_FluidCoolerTwoSpeed_FieldSet, FluidCoolerTwoSpeed>
    {

        private IB_FluidCoolerTwoSpeed_FieldSet() { }


    }
}
