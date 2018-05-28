using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_SetpointManager : IB_HVACObject, IIB_ShareableObj
    {

        public IB_SetpointManager(SetpointManager GhostOSObject) : base(GhostOSObject)
        {
            
        }
        
    }
}
