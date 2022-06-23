using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_SolarCollectorFlatPlateWater : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SolarCollectorFlatPlateWater();

        private static SolarCollectorFlatPlateWater NewDefaultOpsObj(Model model) => new SolarCollectorFlatPlateWater(model);
        public IB_SolarCollectorFlatPlateWater() : base(NewDefaultOpsObj(new Model()))
        {
        }
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_SolarCollectorFlatPlateWater_FieldSet
        : IB_FieldSet<IB_SolarCollectorFlatPlateWater_FieldSet, SolarCollectorFlatPlateWater>
    {
        private IB_SolarCollectorFlatPlateWater_FieldSet() { }
    }
}
