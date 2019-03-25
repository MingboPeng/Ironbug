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
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_CoolingTowerTwoSpeed_FieldSet
        : IB_FieldSet<IB_CoolingTowerTwoSpeed_FieldSet, CoolingTowerTwoSpeed>
    {

        private IB_CoolingTowerTwoSpeed_FieldSet() { }

        public IB_Field HighSpeedNominalCapacity { get; }
            = new IB_BasicField("HighSpeedNominalCapacity", "HCapacity");

        public IB_Field LowSpeedNominalCapacity { get; }
            = new IB_BasicField("LowSpeedNominalCapacity", "LCapacity");


    }
}
