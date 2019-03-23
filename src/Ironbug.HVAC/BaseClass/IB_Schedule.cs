namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_Schedule : IB_ModelObject
    {
        public IB_Schedule(OpenStudio.ModelObject GhostOSObject) : base(GhostOSObject)
        {
            //var a = OpenStudio.ScheduleRuleset
        }
        public abstract OpenStudio.ModelObject ToOS(OpenStudio.Model model);
    }
}