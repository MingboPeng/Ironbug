
using OpenStudio;
using System;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_AirTerminal : IB_ZoneEquipment
    {
        
        public IB_AirTerminal(Func<OpenStudio.Model, HVACComponent> ghostObjInit) : base(ghostObjInit)
        {

        }

        
    }
}
