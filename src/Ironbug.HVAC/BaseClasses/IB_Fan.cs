using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC
{
    public abstract class IB_Fan : IB_HVACObject, IIB_AirLoopObject
    {
        public IB_Fan(HVACComponent GhostOSObject) : base(GhostOSObject)
        {

        }
    }
}
