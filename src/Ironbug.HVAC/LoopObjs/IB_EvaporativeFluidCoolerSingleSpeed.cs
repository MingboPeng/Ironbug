using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_EvaporativeFluidCoolerSingleSpeed : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_EvaporativeFluidCoolerSingleSpeed();

        private static EvaporativeFluidCoolerSingleSpeed NewDefaultOpsObj(Model model) => new EvaporativeFluidCoolerSingleSpeed(model);
        public IB_EvaporativeFluidCoolerSingleSpeed() : base(NewDefaultOpsObj(new Model()))
        {
        }
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_EvaporativeFluidCoolerSingleSpeed_FieldSet
        : IB_FieldSet<IB_EvaporativeFluidCoolerSingleSpeed_FieldSet, EvaporativeFluidCoolerSingleSpeed>
    {

        private IB_EvaporativeFluidCoolerSingleSpeed_FieldSet() { }


    }
}
