using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_BoilerHotWater :IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_BoilerHotWater();

        private static BoilerHotWater NewDefaultOpsObj(Model model) => new BoilerHotWater(model);
        public IB_BoilerHotWater() : base(NewDefaultOpsObj)
        {
        }
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_BoilerHotWater_FieldSet 
        : IB_FieldSet<IB_BoilerHotWater_FieldSet, BoilerHotWater>
    {

        private IB_BoilerHotWater_FieldSet() { }

        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name") {
            };

        public IB_Field FuelType { get; }
            = new IB_BasicField("FuelType", "Fuel");

        public IB_Field NominalCapacity { get; }
            = new IB_BasicField("NominalCapacity", "Capacity");

        public IB_Field NominalThermalEfficiency { get; }
            = new IB_BasicField("NominalThermalEfficiency", "Efficiency");

        public IB_Field NormalizedBoilerEfficiencyCurve { get; }
           = new IB_BasicField("NormalizedBoilerEfficiencyCurve", "EffCurve");
        
    }
}
