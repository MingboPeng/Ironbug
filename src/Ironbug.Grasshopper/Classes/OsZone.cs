namespace Ironbug.Grasshopper
{
    public class OsZone
    {
        public string ZoneName { get; private set; }

        public OsZone(string ZoneName)
        {
            this.ZoneName = ZoneName;
        }

        public override string ToString()
        {
            return ZoneName + "(OsZone)";
        }
    }
}