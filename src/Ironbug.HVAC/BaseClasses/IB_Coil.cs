using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC
{
    public abstract class IB_Coil : IB_HVACComponent, IIB_DualLoopObjects
    {

        public IB_Coil(HVACComponent GhostOSObject) : base(GhostOSObject)
        {

        }
    }
}
