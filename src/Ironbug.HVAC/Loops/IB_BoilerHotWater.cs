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
            return base.ToOS(InitMethod, model);
        }
    }

    public class IB_BoilerHotWater_DataFields : IB_DataFieldSet
    {
        //protected static IddObject RefIddObject => new IdfObject(BoilerHotWater.iddObjectType()).iddObject();
        private static IddObject refIddObject;

        protected static IddObject RefIddObject
        {
            get
            {
                if (refIddObject is null)
                {
                    refIddObject = new IdfObject(BoilerHotWater.iddObjectType()).iddObject();
                }
                return refIddObject;
            }
        }

        protected override Type ParentType => typeof(BoilerHotWater);

        public static readonly IB_DataField Name
            = new IB_DataField("Name", "Name", RefIddObject.GetType(), true) {
            };

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
