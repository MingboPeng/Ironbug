using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingWaterToAirHeatPumpEquationFit : IB_CoilHeatingBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingWaterToAirHeatPumpEquationFit();

        private static CoilHeatingWaterToAirHeatPumpEquationFit NewDefaultOpsObj(Model model) 
            => new CoilHeatingWaterToAirHeatPumpEquationFit(model);
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        

        public IB_CoilHeatingWaterToAirHeatPumpEquationFit() 
            : base(NewDefaultOpsObj)
        {
        }

    }

    public sealed class IB_CoilHeatingWaterToAirHeatPumpEquationFit_FieldSet
        : IB_FieldSet<IB_CoilHeatingWaterToAirHeatPumpEquationFit_FieldSet, CoilHeatingWaterToAirHeatPumpEquationFit>
    {
        private IB_CoilHeatingWaterToAirHeatPumpEquationFit_FieldSet() { }

    }


}
