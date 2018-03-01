using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_BoilerHotWater :IB_ModelObject, IIB_PlantLoopObjects
    {
        private static BoilerHotWater InitMethod(Model model) => new BoilerHotWater(model);
        public IB_BoilerHotWater() : base(InitMethod(new Model()))
        {
        }
        public bool AddToNode(Node node)
        {
            var model = node.model();
            return ((BoilerHotWater)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIB_ModelObject(() => new IB_BoilerHotWater());
        }

        public ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model);
        }
    }

    public class IB_BoilerHotWater_DataFields : IB_DataFieldSet
    {
        protected override IddObject RefIddObject => new BoilerHotWater(new Model()).iddObject();
        //https://openstudio-sdk-documentation.s3.amazonaws.com/cpp/OpenStudio-2.4.0-doc/model/html/classopenstudio_1_1model_1_1_fan_constant_volume.html
        
        protected override Type ParentType => typeof(BoilerHotWater);

        public static readonly IB_DataField Name
            = new IB_DataField("Name", "Name", strType, true);

        public static readonly IB_DataField FuelType
            = new IB_DataField("FuelType", "Fuel", strType, true);

        public static readonly IB_DataField DesignWaterOutletTemperature
            = new IB_DataField("DesignWaterOutletTemperature", "OutWaterT", dbType);

        public static readonly IB_DataField NominalCapacity
            = new IB_DataField("NominalCapacity", "Capacity", dbType);

        public static readonly IB_DataField NominalThermalEfficiency
            = new IB_DataField("NominalThermalEfficiency", "Efficiency", dbType);
        
    }
}
