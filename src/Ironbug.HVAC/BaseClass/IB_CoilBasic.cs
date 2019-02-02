using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_CoilBasic : IB_Coil
    {
        public IB_CoilBasic(HVACComponent GhostOSObject) : base(GhostOSObject)
        {

        }
        //public override HVACComponent ToOS(Model model)
        //{
        //    throw new System.NotImplementedException();
        //}
    }

}
