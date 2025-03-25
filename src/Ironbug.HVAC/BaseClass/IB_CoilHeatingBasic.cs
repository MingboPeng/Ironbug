using OpenStudio;
using System;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_CoilHeatingBasic : IB_CoilBasic
    {
        public IB_CoilHeatingBasic(Func<OpenStudio.Model, HVACComponent> ghostObjInit) : base(ghostObjInit)
        {

        }

        
    }

}
