using OpenStudio;
using System;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_AirLoopHVACUnitary : IB_HVACObject, IIB_AirLoopObject
    {

        public IB_AirLoopHVACUnitary(Func<OpenStudio.Model, HVACComponent> ghostObjInit) : base(ghostObjInit)
        {

        }
    }
}
