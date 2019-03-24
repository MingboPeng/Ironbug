using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingWaterToAirHeatPumpEquationFit : IB_CoilHeatingBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingWaterToAirHeatPumpEquationFit();
        private static CoilCoolingWaterToAirHeatPumpEquationFit NewDefaultOpsObj(Model model) 
            => new CoilCoolingWaterToAirHeatPumpEquationFit(model);
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        

        public IB_CoilCoolingWaterToAirHeatPumpEquationFit() 
            : base(NewDefaultOpsObj(new Model()))
        {
        }

    }

    public sealed class IB_CoilCoolingWaterToAirHeatPumpEquationFit_FieldSet
        : IB_FieldSet<IB_CoilCoolingWaterToAirHeatPumpEquationFit_FieldSet, CoilCoolingWaterToAirHeatPumpEquationFit>
    {
        private IB_CoilCoolingWaterToAirHeatPumpEquationFit_FieldSet() { }

    }


}
