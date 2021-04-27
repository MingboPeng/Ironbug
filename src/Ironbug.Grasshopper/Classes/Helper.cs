using Grasshopper.Kernel.Types;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.Grasshopper
{
    public static class Helper
    {
        /// For new Honeybee
        public static IEnumerable<string> FromLBTRooms(IEnumerable<object> inRooms)
        {
            var rooms = inRooms.OfType<GH_ObjectWrapper>().Select(_ => _.Value);
            var pyRun = Rhino.Runtime.PythonScript.Create();
            pyRun.SetVariable("HBRooms", rooms.ToArray());
            string pyScript = @"
PyHBObjects=[];
for room in HBRooms:
    PyHBObjects.append(room.identifier);
        ";
            pyRun.ExecuteScript(pyScript);
            var PyHBObjects = pyRun.GetVariable("PyHBObjects") as IList<dynamic>;

            var zoneIds = PyHBObjects.Select(_ => _ as string);
            return zoneIds;
        }

        /// For legacy Honeybee
        public static IEnumerable<string> CallFromHBHive(IEnumerable<GH_Brep> inBreps)
        {
            var HBIDs = new List<string>();
            foreach (var item in inBreps)
            {
                if (inBreps is null) continue;

                item.Value.UserDictionary.TryGetString("HBID", out string HBID);
                if (string.IsNullOrEmpty(HBID)) continue;

                HBIDs.Add(HBID);
            }

            if (HBIDs.Any())
            {
                return GetHBObjects(HBIDs).Select(_ => _ as string);
            }
            else
            {
                return new List<string>();
            }
        }

        private static IList<dynamic> GetHBObjects(List<string> HBIDs)
        {
            var pyRun = Rhino.Runtime.PythonScript.Create();
            pyRun.SetVariable("HBIDs", HBIDs.ToArray());
            string pyScript = @"
import scriptcontext as sc;
PyHBObjects=[];
for HBID in HBIDs:
    baseKey, key = HBID.split('#')[0], '#'.join(HBID.split('#')[1:])
    HBZone = sc.sticky['HBHive'][baseKey][key];
    PyHBObjects.append(HBZone.name);
";

            pyRun.ExecuteScript(pyScript);
            var PyHBObjects = pyRun.GetVariable("PyHBObjects") as IList<dynamic>;

            return PyHBObjects;
        }
    }


   
}