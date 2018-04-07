using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_BoilerHotWater :IB_HVACObject, IIB_PlantLoopObjects
    {
        private static BoilerHotWater InitMethod(Model model) => new BoilerHotWater(model);
        public IB_BoilerHotWater() : base(InitMethod(new Model()))
        {
        }
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((BoilerHotWater)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_BoilerHotWater());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_BoilerHotWater().get();
        }
    }

    public sealed class IB_BoilerHotWater_DataFields 
        : IB_DataFieldSet<IB_BoilerHotWater_DataFields, BoilerHotWater>
    {

        private IB_BoilerHotWater_DataFields() { }

        public IB_DataField Name { get; }
            = new IB_BasicDataField("Name", "Name") {
            };

        public IB_DataField FuelType { get; }
            = new IB_BasicDataField("FuelType", "Fuel");

        public IB_DataField DesignWaterOutletTemperature { get; }
            = new IB_ProDataField("DesignWaterOutletTemperature", "OutWaterT");

        public IB_DataField NominalCapacity { get; }
            = new IB_ProDataField("NominalCapacity", "Capacity");

        public IB_DataField NominalThermalEfficiency { get; }
            = new IB_ProDataField("NominalThermalEfficiency", "Efficiency");

       
    }
}
