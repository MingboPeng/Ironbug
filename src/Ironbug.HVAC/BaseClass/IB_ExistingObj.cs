
namespace Ironbug.HVAC.BaseClass
{
    public class IB_ExistingObj
    {
        public string Name { get; private set; }
        public string OsmFile { get; private set; }

        public IB_ExistingObj(string ExistingAirloopName, string ExistingOsmPath)
        {

            this.Name = ExistingAirloopName;
            this.OsmFile = ExistingOsmPath;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
