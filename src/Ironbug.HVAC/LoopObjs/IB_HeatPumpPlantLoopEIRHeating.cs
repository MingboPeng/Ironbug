using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_HeatPumpPlantLoopEIRHeating : IB_HVACObject, IIB_PlantLoopObjects, IIB_DualLoopObj
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_HeatPumpPlantLoopEIRHeating();

        private static HeatPumpPlantLoopEIRHeating NewDefaultOpsObj(Model model) => new HeatPumpPlantLoopEIRHeating(model);

        private IB_HeatPumpPlantLoopEIRCooling _coolingHP => this.GetChild<IB_HeatPumpPlantLoopEIRCooling>(0);

        public IB_HeatPumpPlantLoopEIRHeating() : base(NewDefaultOpsObj)
        {
        }

        public void SetCompanionCoolingHeatPump(IB_HeatPumpPlantLoopEIRCooling heatingHP)
        {
            this.SetChild(heatingHP);
        }

        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            if (_coolingHP != null)
            {
                var hp = _coolingHP?.ToOS(model) as HeatPumpPlantLoopEIRCooling;
                obj.setCompanionCoolingHeatPump(hp);
            }

            return obj;
        }
    }

    public sealed class IB_HeatPumpPlantLoopEIRHeating_FieldSet
      : IB_FieldSet<IB_HeatPumpPlantLoopEIRHeating_FieldSet, HeatPumpPlantLoopEIRHeating>
    {

        private IB_HeatPumpPlantLoopEIRHeating_FieldSet() { }

    }
}
