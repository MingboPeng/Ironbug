namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_Schedule : IB_ModelObject
    {
        public IB_Schedule(OpenStudio.Schedule GhostOSObject) : base(GhostOSObject)
        {
            //var a = OpenStudio.ScheduleRuleset
        }
        public virtual OpenStudio.ModelObject ToOS(OpenStudio.Model model)
        {
            return this.InitOpsObj(model);
        }
    }
}