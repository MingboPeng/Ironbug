using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_Curve: IB_ModelObject
    {
        public IB_Curve(ModelObject ghostOSObject) : base(ghostOSObject)
        {
        }

        public Curve ToOS()
        {
            return this.NewOpsObj(new Model()).to_Curve().get();
        }
    }
}
