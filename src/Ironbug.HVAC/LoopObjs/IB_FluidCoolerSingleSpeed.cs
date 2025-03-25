using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_FluidCoolerSingleSpeed : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_FluidCoolerSingleSpeed();

        private static FluidCoolerSingleSpeed NewDefaultOpsObj(Model model) => new FluidCoolerSingleSpeed(model);
        public IB_FluidCoolerSingleSpeed() : base(NewDefaultOpsObj)
        {
        }
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_FluidCoolerSingleSpeed_FieldSet
        : IB_FieldSet<IB_FluidCoolerSingleSpeed_FieldSet, FluidCoolerSingleSpeed>
    {

        private IB_FluidCoolerSingleSpeed_FieldSet() { }


    }
}
