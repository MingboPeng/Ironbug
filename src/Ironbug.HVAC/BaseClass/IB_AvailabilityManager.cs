using OpenStudio;
using System;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_AvailabilityManager : IB_ModelObject
    {
        public IB_AvailabilityManager(Func<OpenStudio.Model, ModelObject> ghostObjInit) : base(ghostObjInit)
        {
        }
        public abstract OpenStudio.AvailabilityManager ToOS(OpenStudio.Model model);
    }
}