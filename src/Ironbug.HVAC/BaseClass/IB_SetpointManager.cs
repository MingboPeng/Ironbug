using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_SetpointManager : IB_HVACObject, IIB_AirLoopObject, IIB_PlantLoopObjects
    {

        public IB_SetpointManager(Func<OpenStudio.Model, SetpointManager> ghostObjInit) : base(ghostObjInit)
        {
            
        }
        
    }
}
