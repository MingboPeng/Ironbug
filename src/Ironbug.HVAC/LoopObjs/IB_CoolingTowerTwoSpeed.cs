using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoolingTowerTwoSpeed : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoolingTowerTwoSpeed();

        private static CoolingTowerTwoSpeed InitMethod(Model model) => new CoolingTowerTwoSpeed(model);
        public IB_CoolingTowerTwoSpeed() : base(InitMethod(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoolingTowerTwoSpeed)this.ToOS(model)).addToNode(node);
        }
        
        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CoolingTowerTwoSpeed().get();
        }
    }

    public sealed class IB_CoolingTowerTwoSpeed_DataFields
        : IB_DataFieldSet<IB_CoolingTowerTwoSpeed_DataFields, CoolingTowerTwoSpeed>
    {

        private IB_CoolingTowerTwoSpeed_DataFields() { }

        public IB_DataField HighSpeedNominalCapacity { get; }
            = new IB_BasicDataField("HighSpeedNominalCapacity", "HCapacity");

        public IB_DataField LowSpeedNominalCapacity { get; }
            = new IB_BasicDataField("LowSpeedNominalCapacity", "LCapacity");


    }
}
