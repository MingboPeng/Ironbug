using OpenStudio;
using System;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_CoilCoolingBasic : IB_CoilBasic
    {
        public IB_CoilCoolingBasic(Func<OpenStudio.Model, HVACComponent> ghostObjInit) : base(ghostObjInit)
        {

        }
    }

}
