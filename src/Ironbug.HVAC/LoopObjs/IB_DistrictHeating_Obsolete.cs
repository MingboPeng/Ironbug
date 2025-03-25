using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    [Obsolete("Use IB_DistrictHeatingWater instead.", true)]
    public class IB_DistrictHeating : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_DistrictHeating();

        private static DistrictHeatingWater NewDefaultOpsObj(Model model) => new DistrictHeatingWater(model);
        public IB_DistrictHeating() : base(NewDefaultOpsObj)
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_DistrictHeating_FieldSet
        : IB_FieldSet<IB_DistrictHeating_FieldSet, DistrictHeatingWater>
    {
        private IB_DistrictHeating_FieldSet() { }
        
    }
}
