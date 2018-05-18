using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoolingTowerVariableSpeed : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoolingTowerVariableSpeed();

        private static CoolingTowerVariableSpeed InitMethod(Model model) => new CoolingTowerVariableSpeed(model);
        public IB_CoolingTowerVariableSpeed() : base(InitMethod(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoolingTowerVariableSpeed)this.ToOS(model)).addToNode(node);
        }

        //public override IB_ModelObject Duplicate()
        //{
        //    return base.DuplicateIBObj(() => new IB_CoolingTowerVariableSpeed());
        //}

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CoolingTowerVariableSpeed().get();
        }
    }

    public sealed class IB_CoolingTowerVariableSpeed_DataFields
        : IB_DataFieldSet<IB_CoolingTowerVariableSpeed_DataFields, CoolingTowerVariableSpeed>
    {

        private IB_CoolingTowerVariableSpeed_DataFields() { }

        public IB_DataField DesignWaterFlowRate { get; }
            = new IB_BasicDataField("DesignWaterFlowRate", "WaterFlowRate");

        public IB_DataField DesignAirFlowRate { get; }
            = new IB_BasicDataField("DesignAirFlowRate", "AirFlowRate");

        public IB_DataField DesignFanPower { get; }
            = new IB_BasicDataField("DesignFanPower", "FanPower");


    }
}
