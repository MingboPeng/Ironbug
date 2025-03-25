using System;
using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_Coil : IB_HVACObject, IIB_AirLoopObject
    {

        public IB_Coil(Func<OpenStudio.Model, HVACComponent> ghostObjInit) : base(ghostObjInit)
        {

        }
    }
}
