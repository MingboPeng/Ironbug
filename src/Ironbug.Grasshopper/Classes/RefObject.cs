using System.Collections.Generic;

namespace Ironbug.Grasshopper
{
    public class RefObject
    {
        public string Name { get; private set; }

        public string OsString { get; private set; }

        public List<string> ChildrenString { get; private set; } = new List<string>();

        public RefObject(string Name, string osString)
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
            return ChildrenString.Count > 0 ? $"{Name} ({ChildrenString.Count} Children)" : Name;
        }
    }
}