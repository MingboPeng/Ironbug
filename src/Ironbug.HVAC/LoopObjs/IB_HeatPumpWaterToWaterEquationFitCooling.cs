using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_HeatPumpWaterToWaterEquationFitCooling : IB_HVACObject, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_HeatPumpWaterToWaterEquationFitCooling();

        private static HeatPumpWaterToWaterEquationFitCooling NewDefaultOpsObj(Model model) => new HeatPumpWaterToWaterEquationFitCooling(model);

        public IB_HeatPumpWaterToWaterEquationFitCooling():base(NewDefaultOpsObj)
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }

      
    }

    public sealed class IB_HeatPumpWaterToWaterEquationFitCooling_FieldSet
        : IB_FieldSet<IB_HeatPumpWaterToWaterEquationFitCooling_FieldSet, HeatPumpWaterToWaterEquationFitCooling>
    {
        private IB_HeatPumpWaterToWaterEquationFitCooling_FieldSet() {}
        
    }
}
