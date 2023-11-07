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
                
                var pyObj = wrapper.Value;
                if (pyObj is Ironbug.HVAC.BaseClass.IB_ThermalZone ibZone)
                {
                    ibZone.CustomAttributes.TryGetValue(Ironbug.HVAC.BaseClass.IB_ThermalZone_FieldSet.Value.Name, out var ibName);
                    return ibName?.ToString();
                }

                // LBT Room
                var isPythonObj = pyObj.GetType().ToString().StartsWith("IronPython.");
                if (!isPythonObj)
                    throw new System.ArgumentException("Invalid Room object!");

                var name = pyObj.ToString();
                var isLBTRoom = name.StartsWith("Room:");
                var isDFRoom2D = name.StartsWith("Room2D:");

                if (isLBTRoom || isDFRoom2D)
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

        public static int[] GetDateRange(object LBObj)
        {
            if (LBObj is null)
                throw new System.ArgumentNullException();

            if (LBObj is GH_ObjectWrapper wrapper)
            {
                // LBT Room
                var pyObj = wrapper.Value;
                var isPythonObj = pyObj.GetType().ToString().StartsWith("IronPython.");
                if (!isPythonObj)
                    throw new System.ArgumentException("Invalid LB Analysis Period!");

                return GetLBTDateRange(pyObj);
            }

            throw new System.ArgumentException("Failed to convert LB Analysis Period!");
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

        static int[] GetLBTDateRange(object lbtAnalysisPeriod)
        {
            var pyRun = GetPython();
            pyRun.SetVariable("ap", lbtAnalysisPeriod);
            string pyScript = @"
dateRange = [ ap.st_month, ap.st_day, ap.end_month, ap.end_day]
";
            pyRun.ExecuteScript(pyScript);
            var pyobj = pyRun.GetVariable("dateRange") as IList<dynamic>;
            var dateRange = pyobj.OfType<int>().ToArray();
            return dateRange;
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