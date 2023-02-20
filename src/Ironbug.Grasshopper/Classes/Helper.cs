using Grasshopper.Kernel.Types;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.Grasshopper
{
    public static class Helper
    {

        static Rhino.Runtime.PythonScript _pythonRuntime = null;
        public static Rhino.Runtime.PythonScript GetPython()
        {
            _pythonRuntime = _pythonRuntime ?? Rhino.Runtime.PythonScript.Create();
            return _pythonRuntime;
        }

        public static IEnumerable<string> GetRoomNames(List<object> HBZonesOrNames)
        {
            return HBZonesOrNames?.Select(x => GetRoomName(x))?.ToList();
        }

        public static string GetRoomName(object HBObj)
        {
            if (HBObj is null)
                return string.Empty;

            if (HBObj is Types.OsZone z)
            {
                return z.ZoneName;
            }
            else if (HBObj is GH_Brep b)
            {
                // Legacy HBZones
                throw new System.ArgumentException("Ironbug doesn't support HBZones from the legacy version anymore!");
            }
            else if (HBObj is GH_String s)
            {
                return s.Value.ToString();
            }
            else if (HBObj is GH_ObjectWrapper wrapper)
            {
                // LBT Room
                var pyObj = wrapper.Value;
                var isLBTRoom = pyObj.ToString().StartsWith("Room:");
                var tp = pyObj.GetType();
                isLBTRoom &= tp.ToString().StartsWith("IronPython.");

                if (isLBTRoom) 
                    return GetLBTRoomName(pyObj);

            }
            else if (HBObj.GetType().Name == "GH_HBPythonObjectGoo") //Pollination Honeybee components
            {
                var rm = HBObj;
                var tp = HBObj?.GetType();
                var type = tp?.GetProperty("POTypeName")?.GetValue(rm)?.ToString();

                if (type == "Room")
                {
                    var pyObj = tp.GetProperty("RefPythonObj")?.GetValue(rm);
                    return GetLBTRoomName(pyObj);
                }
            }


            throw new System.ArgumentException("Invalid Room object!");

        }

        /// For new Honeybee
        static string GetLBTRoomName(object lbtRoom)
        {
            var pyRun = GetPython();
            pyRun.SetVariable("HBRoom", lbtRoom);
            string pyScript = @"
id = HBRoom.identifier
";
            pyRun.ExecuteScript(pyScript);
            var name = pyRun.GetVariable("id") as string;
            return name;
        }

        /// For new Honeybee
        public static IEnumerable<string> FromLBTRooms(IEnumerable<object> inRooms)
        {
            var rooms = inRooms.OfType<GH_ObjectWrapper>().Select(_ => _.Value);
            var pyRun = GetPython();
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
            var pyRun = GetPython();
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