using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoolingTowerSingleSpeed : IB_HVACObject, IIB_PlantLoopObjects
    {
        private static CoolingTowerSingleSpeed InitMethod(Model model) => new CoolingTowerSingleSpeed(model);
        public IB_CoolingTowerSingleSpeed() : base(InitMethod(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoolingTowerSingleSpeed)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_CoolingTowerSingleSpeed());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_CoolingTowerSingleSpeed().get();
        }
    }

    public sealed class IB_CoolingTowerSingleSpeed_DataFields
        : IB_DataFieldSet<IB_CoolingTowerSingleSpeed_DataFields, CoolingTowerSingleSpeed>
    {

        private IB_CoolingTowerSingleSpeed_DataFields() { }

        public IB_DataField NominalCapacity { get; }
            = new IB_BasicDataField("NominalCapacity", "Capacity");


    }
}
