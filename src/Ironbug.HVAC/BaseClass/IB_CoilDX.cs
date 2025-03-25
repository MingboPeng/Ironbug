using OpenStudio;
using System;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_CoilDX : IB_Coil
    {
        public IB_CoilDX(Func<OpenStudio.Model, HVACComponent> ghostObjInit) : base(ghostObjInit)
        {

        }
    }

}
