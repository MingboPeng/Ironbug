using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_EvaporativeFluidCoolerTwoSpeed : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_EvaporativeFluidCoolerTwoSpeed();

        private static EvaporativeFluidCoolerTwoSpeed NewDefaultOpsObj(Model model) => new EvaporativeFluidCoolerTwoSpeed(model);
        public IB_EvaporativeFluidCoolerTwoSpeed() : base(NewDefaultOpsObj)
        {
        }
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_EvaporativeFluidCoolerTwoSpeed_FieldSet
        : IB_FieldSet<IB_EvaporativeFluidCoolerTwoSpeed_FieldSet, EvaporativeFluidCoolerTwoSpeed>
    {

        private IB_EvaporativeFluidCoolerTwoSpeed_FieldSet() { }


    }
}
