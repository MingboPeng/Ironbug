using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_HeatPumpWaterToWaterEquationFitHeating : IB_HVACObject, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_HeatPumpWaterToWaterEquationFitHeating();

        private static HeatPumpWaterToWaterEquationFitHeating NewDefaultOpsObj(Model model) => new HeatPumpWaterToWaterEquationFitHeating(model);

        public IB_HeatPumpWaterToWaterEquationFitHeating():base(NewDefaultOpsObj(new Model()))
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }

      
    }

    public sealed class IB_HeatPumpWaterToWaterEquationFitHeating_FieldSet
        : IB_FieldSet<IB_HeatPumpWaterToWaterEquationFitHeating_FieldSet, HeatPumpWaterToWaterEquationFitHeating>
    {
        private IB_HeatPumpWaterToWaterEquationFitHeating_FieldSet() {}
        
    }
}
