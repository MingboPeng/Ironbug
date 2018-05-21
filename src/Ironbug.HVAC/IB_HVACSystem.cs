using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_HVACSystem
    {
        public IB_AirLoopHVAC AirLoop { get; private set; }
        public IB_PlantLoop PlantLoop { get; private set; }
        public IB_AirConditionerVariableRefrigerantFlow VariableRefrigerantFlow { get; private set; }

        public IB_HVACSystem(IB_AirLoopHVAC airLoop, IB_PlantLoop plantLoop, IB_AirConditionerVariableRefrigerantFlow vrf)
        {
            this.AirLoop = airLoop;
            this.PlantLoop = plantLoop;
            this.VariableRefrigerantFlow = vrf;
        }



    }
}
