using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_SolarCollectorPerformanceFlatPlate : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SolarCollectorPerformanceFlatPlate();

        private static SolarCollectorPerformanceFlatPlate NewDefaultOpsObj(Model model) => new SolarCollectorFlatPlateWater(model)?.solarCollectorPerformance();

        public IB_SolarCollectorPerformanceFlatPlate() : base(NewDefaultOpsObj)
        {
        }

    }


    public sealed class IB_SolarCollectorPerformanceFlatPlate_FieldSet
        : IB_FieldSet<IB_SolarCollectorPerformanceFlatPlate_FieldSet, SolarCollectorPerformanceFlatPlate>
    {
        private IB_SolarCollectorPerformanceFlatPlate_FieldSet() { }
    }
}
