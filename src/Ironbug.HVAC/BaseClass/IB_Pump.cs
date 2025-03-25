using OpenStudio;
using System;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_Pump: IB_HVACObject, IIB_PlantLoopObjects
    {
        public IB_Pump(Func<OpenStudio.Model, HVACComponent> ghostObjInit) : base(ghostObjInit)
        {
            
        }
    }
}
