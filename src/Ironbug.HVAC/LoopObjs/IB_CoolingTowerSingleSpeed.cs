using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoolingTowerSingleSpeed : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoolingTowerSingleSpeed();

        private static CoolingTowerSingleSpeed NewDefaultOpsObj(Model model) => new CoolingTowerSingleSpeed(model);
        public IB_CoolingTowerSingleSpeed() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoolingTowerSingleSpeed)this.ToOS(model)).addToNode(node);
        }
        

        protected override ModelObject NewOpsObj(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model).to_CoolingTowerSingleSpeed().get();
        }
    }

    public sealed class IB_CoolingTowerSingleSpeed_DataFields
        : IB_FieldSet<IB_CoolingTowerSingleSpeed_DataFields, CoolingTowerSingleSpeed>
    {

        private IB_CoolingTowerSingleSpeed_DataFields() { }

        public IB_Field NominalCapacity { get; }
            = new IB_BasicField("NominalCapacity", "Capacity");


    }
}
