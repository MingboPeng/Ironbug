using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilWaterHeatingAirToWaterHeatPump: IB_CoilDX
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilWaterHeatingAirToWaterHeatPump();
        private static CoilWaterHeatingAirToWaterHeatPump NewDefaultOpsObj(Model model) => new CoilWaterHeatingAirToWaterHeatPump(model);

        public IB_CoilWaterHeatingAirToWaterHeatPump() : base(NewDefaultOpsObj)
        {
        }
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }

    }

    public sealed class IB_CoilWaterHeatingAirToWaterHeatPump_FieldSet
        : IB_FieldSet<IB_CoilWaterHeatingAirToWaterHeatPump_FieldSet, CoilWaterHeatingAirToWaterHeatPump>
    {
        private IB_CoilWaterHeatingAirToWaterHeatPump_FieldSet() { }

    }
}
