namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_AvailabilityManager : IB_ModelObject
    {
        public IB_AvailabilityManager(OpenStudio.ModelObject GhostOSObject) : base(GhostOSObject)
        {
        }
        public abstract OpenStudio.AvailabilityManager ToOS(OpenStudio.Model model);
    }
}