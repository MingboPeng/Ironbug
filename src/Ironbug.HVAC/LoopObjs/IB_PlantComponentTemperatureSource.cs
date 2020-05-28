using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;


namespace Ironbug.HVAC
{
    public class IB_PlantComponentTemperatureSource: IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_PlantComponentTemperatureSource();

        private static PlantComponentTemperatureSource NewDefaultOpsObj(Model model) => new PlantComponentTemperatureSource(model);
        public IB_PlantComponentTemperatureSource() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }
    public sealed class IB_PlantComponentTemperatureSource_FieldSet
        : IB_FieldSet<IB_PlantComponentTemperatureSource_FieldSet, PlantComponentTemperatureSource>
    {
        private IB_PlantComponentTemperatureSource_FieldSet() { }
    }

}
