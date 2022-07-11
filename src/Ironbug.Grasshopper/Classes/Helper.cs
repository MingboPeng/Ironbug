using Grasshopper.Kernel.Types;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.Grasshopper
{
    public static class Helper
    {

        public static IEnumerable<string> GetRoomNames(List<object> HBZonesOrNames)
        {
            var firstItem = HBZonesOrNames?.FirstOrDefault();
            var zoneNames = new List<string>();

            if (firstItem == null)
                return zoneNames;

            if (firstItem is Types.OsZone)
            {
                zoneNames = HBZonesOrNames.Where(_ => _ is Types.OsZone).Select(_ => (_ as Types.OsZone).ZoneName).ToList<string>();
            }
            else if (firstItem is GH_Brep)
            {
                // Legacy HBZones
                var hbzones = HBZonesOrNames.SkipWhile(_ => _ is null || _ is Types.OsZone).Select(_ => _ as GH_Brep);
                zoneNames = Helper.CallFromHBHive(hbzones).ToList();
            }
            else if (firstItem is GH_String)
            {
                zoneNames = HBZonesOrNames.Select(_ => (_ as GH_String).Value).ToList<string>();
            }
            else if (firstItem is GH_ObjectWrapper wrapper)
            {
                // LBT Room
                var isLBTRoom = wrapper.Value.ToString().StartsWith("Room:");
                isLBTRoom &= wrapper.Value.GetType().ToString().StartsWith("IronPython.");

                if (isLBTRoom)
                {
                    zoneNames = Helper.FromLBTRooms(HBZonesOrNames).ToList();
                }

            }
            else if (firstItem.GetType().Name == "GH_HBPythonObjectGoo") //Pollination Honeybee components
            {
                var rm = HBZonesOrNames[0];
                var tp = rm?.GetType();
                var type = tp?.GetProperty("POTypeName")?.GetValue(rm)?.ToString();

                if (type == "Room")
                {
                    var pyObjs = HBZonesOrNames.Select(_ => tp.GetProperty("RefPythonObj")?.GetValue(rm)).Select(_ => new GH_ObjectWrapper(_));
                    zoneNames = Helper.FromLBTRooms(pyObjs).ToList();
                }
            }

            return zoneNames;
        }

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