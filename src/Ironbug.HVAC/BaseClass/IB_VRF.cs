using OpenStudio;
using System;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_VRFSystem : IB_HVACObject
    {
        public IB_VRFSystem(Func<OpenStudio.Model, HVACComponent> ghostObjInit) : base(ghostObjInit)
        {
        }

        //public abstract HVACComponent ToOS(Model model);
       
    }
}