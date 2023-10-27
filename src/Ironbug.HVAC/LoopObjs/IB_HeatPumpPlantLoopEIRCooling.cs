using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{

    public class IB_HeatPumpPlantLoopEIRCooling : IB_HVACObject, IIB_PlantLoopObjects, IIB_DualLoopObj
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_HeatPumpPlantLoopEIRCooling();

        private static HeatPumpPlantLoopEIRCooling NewDefaultOpsObj(Model model) => new HeatPumpPlantLoopEIRCooling(model);

        private IB_HeatPumpPlantLoopEIRHeating _heatingHP => this.GetChild<IB_HeatPumpPlantLoopEIRHeating>(0);

        public IB_HeatPumpPlantLoopEIRCooling() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public void SetCompanionHeatingHeatPump(IB_HeatPumpPlantLoopEIRHeating heatingHP)
        {
            this.SetChild(heatingHP);
        }

        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            if (_heatingHP != null)
            {
                var hp = _heatingHP?.ToOS(model) as HeatPumpPlantLoopEIRHeating;
                obj.setCompanionHeatingHeatPump(hp);
            }
            return obj;
        }
    }

    public sealed class IB_HeatPumpPlantLoopEIRCooling_FieldSet
        : IB_FieldSet<IB_HeatPumpPlantLoopEIRCooling_FieldSet, HeatPumpPlantLoopEIRCooling>
    {

        private IB_HeatPumpPlantLoopEIRCooling_FieldSet() { }

    }
}
