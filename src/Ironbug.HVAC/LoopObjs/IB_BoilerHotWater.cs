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
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_BoilerHotWater();

        private static BoilerHotWater InitMethod(Model model) => new BoilerHotWater(model);
        public IB_BoilerHotWater() : base(InitMethod(new Model()))
        {
        }
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((BoilerHotWater)this.ToOS(model)).addToNode(node);
        }

        //public override IB_ModelObject Duplicate()
        //{
        //    return base.DuplicateIBObj(IB_InitSelf);
        //}

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_BoilerHotWater().get();
        }
    }

    public sealed class IB_BoilerHotWater_DataFields 
        : IB_FieldSet<IB_BoilerHotWater_DataFields, BoilerHotWater>
    {

        private IB_BoilerHotWater_DataFields() { }

        public IB_Field Name { get; }
            = new IB_BasicDataField("Name", "Name") {
            };

        public IB_Field FuelType { get; }
            = new IB_BasicDataField("FuelType", "Fuel");

        public IB_Field DesignWaterOutletTemperature { get; }
            = new IB_ProDataField("DesignWaterOutletTemperature", "OutWaterT");

        public IB_Field NominalCapacity { get; }
            = new IB_ProDataField("NominalCapacity", "Capacity");

        public IB_Field NominalThermalEfficiency { get; }
            = new IB_ProDataField("NominalThermalEfficiency", "Efficiency");

       
    }
}
