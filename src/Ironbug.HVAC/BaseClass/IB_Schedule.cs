using OpenStudio;
using System;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_Schedule : IB_ModelObject
    {
        public IB_Schedule(Func<OpenStudio.Model, ModelObject> ghostObjInit) : base(ghostObjInit)
        {
            //var a = OpenStudio.ScheduleRuleset
        }
        public abstract OpenStudio.ModelObject ToOS(OpenStudio.Model model);
    }
}