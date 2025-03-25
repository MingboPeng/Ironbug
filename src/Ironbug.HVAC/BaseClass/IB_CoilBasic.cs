using OpenStudio;
using System;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_CoilBasic : IB_Coil
    {
        public IB_CoilBasic(Func<OpenStudio.Model, HVACComponent> ghostObjInit) : base(ghostObjInit)
        {

        }
        //public override HVACComponent ToOS(Model model)
        //{
        //    throw new System.NotImplementedException();
        //}
    }

}
