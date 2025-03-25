using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{

    public class IB_DistrictHeatingWater : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_DistrictHeatingWater();

        private static DistrictHeatingWater NewDefaultOpsObj(Model model) => new DistrictHeatingWater(model);
        public IB_DistrictHeatingWater() : base(NewDefaultOpsObj)
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_DistrictHeatingWater_FieldSet
        : IB_FieldSet<IB_DistrictHeatingWater_FieldSet, DistrictHeatingWater>
    {
        private IB_DistrictHeatingWater_FieldSet() { }
        
        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");
        public IB_Field NominalCapacity { get; }
            = new IB_BasicField("NominalCapacity", "Capacity");

    }
}
