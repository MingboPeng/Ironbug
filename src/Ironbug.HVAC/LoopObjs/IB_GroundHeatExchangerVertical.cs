using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_GroundHeatExchangerVertical : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_GroundHeatExchangerVertical();
        private static GroundHeatExchangerVertical NewDefaultOpsObj(Model model) => new GroundHeatExchangerVertical(model);
        public IB_GroundHeatExchangerVertical():base(NewDefaultOpsObj(new Model()))
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_GroundHeatExchangerVertical_FieldSet
        : IB_FieldSet<IB_GroundHeatExchangerVertical_FieldSet, GroundHeatExchangerVertical>
    {
        private IB_GroundHeatExchangerVertical_FieldSet()
        {
        }
    }
}
