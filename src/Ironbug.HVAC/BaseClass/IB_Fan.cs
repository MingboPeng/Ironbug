using OpenStudio;
using System;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_Fan : IB_HVACObject, IIB_AirLoopObject
    {

        public IB_Fan(Func<OpenStudio.Model, HVACComponent> ghostObjInit) : base(ghostObjInit)
        {
        }
    }
    
}
