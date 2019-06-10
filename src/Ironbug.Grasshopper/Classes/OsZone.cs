using System.Collections.Generic;

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

    public class OsObject
    {
        public string Name { get; private set; }

        public string OsString { get; private set; }

        public List<string> ChildrenString { get; private set; } = new List<string>();

        public OsObject(string Name, string osString)
        {
            this.Name = Name;
            this.OsString = osString;
        }

        public void AddChild(string ChildString)
        {
            ChildrenString.Add(ChildString);
        }


        public override string ToString()
        {
            //return OsString;
            return ChildrenString.Count > 0 ? $"{Name} \n\twith {ChildrenString.Count} Children" : Name;
        }
    }
}