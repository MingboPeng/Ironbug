using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_AirTerminal : IB_ZoneEquipment
    {
        
        public IB_AirTerminal(HVACComponent GhostOSObject) : base(GhostOSObject)
        {

        }
    }
}
