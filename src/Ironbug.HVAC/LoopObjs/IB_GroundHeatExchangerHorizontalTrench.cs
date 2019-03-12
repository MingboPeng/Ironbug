using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_GroundHeatExchangerHorizontalTrench : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_GroundHeatExchangerHorizontalTrench();
        private static GroundHeatExchangerHorizontalTrench NewDefaultOpsObj(Model model) => new GroundHeatExchangerHorizontalTrench(model);
        public IB_GroundHeatExchangerHorizontalTrench():base(NewDefaultOpsObj(new Model()))
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_GroundHeatExchangerHorizontalTrench_FieldSet
        : IB_FieldSet<IB_GroundHeatExchangerHorizontalTrench_FieldSet, GroundHeatExchangerHorizontalTrench>
    {
        private IB_GroundHeatExchangerHorizontalTrench_FieldSet()
        {
        }
    }
}
