using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoolingTowerVariableSpeed : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoolingTowerVariableSpeed();

        private static CoolingTowerVariableSpeed NewDefaultOpsObj(Model model) => new CoolingTowerVariableSpeed(model);
        public IB_CoolingTowerVariableSpeed() : base(NewDefaultOpsObj)
        {
        }


        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_CoolingTowerVariableSpeed_FieldSet
        : IB_FieldSet<IB_CoolingTowerVariableSpeed_FieldSet, CoolingTowerVariableSpeed>
    {

        private IB_CoolingTowerVariableSpeed_FieldSet() { }

        public IB_Field DesignWaterFlowRate { get; }
            = new IB_BasicField("DesignWaterFlowRate", "WaterFlowRate");

        public IB_Field DesignAirFlowRate { get; }
            = new IB_BasicField("DesignAirFlowRate", "AirFlowRate");

        public IB_Field DesignFanPower { get; }
            = new IB_BasicField("DesignFanPower", "FanPower");


    }
}
