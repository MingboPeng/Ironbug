using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoolingTowerTwoSpeed : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoolingTowerTwoSpeed();

        private static CoolingTowerTwoSpeed NewDefaultOpsObj(Model model) => new CoolingTowerTwoSpeed(model);
        public IB_CoolingTowerTwoSpeed() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoolingTowerTwoSpeed)this.ToOS(model)).addToNode(node);
        }
        
        protected override ModelObject NewOpsObj(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model).to_CoolingTowerTwoSpeed().get();
        }
    }

    public sealed class IB_CoolingTowerTwoSpeed_DataFields
        : IB_FieldSet<IB_CoolingTowerTwoSpeed_DataFields, CoolingTowerTwoSpeed>
    {

        private IB_CoolingTowerTwoSpeed_DataFields() { }

        public IB_Field HighSpeedNominalCapacity { get; }
            = new IB_BasicField("HighSpeedNominalCapacity", "HCapacity");

        public IB_Field LowSpeedNominalCapacity { get; }
            = new IB_BasicField("LowSpeedNominalCapacity", "LCapacity");


    }
}
