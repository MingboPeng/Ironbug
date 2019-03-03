using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_HeatExchangerFluidToFluid : IB_HVACObject, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_HeatExchangerFluidToFluid();

        private static HeatExchangerFluidToFluid NewDefaultOpsObj(Model model) => new HeatExchangerFluidToFluid(model);

        public IB_HeatExchangerFluidToFluid() : base(NewDefaultOpsObj(new Model()))
        {
            
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_HeatExchangerFluidToFluid_FieldSet
    : IB_FieldSet<IB_HeatExchangerFluidToFluid_FieldSet, HeatExchangerFluidToFluid>
    {
        private IB_HeatExchangerFluidToFluid_FieldSet()
        {
        }
    }
}