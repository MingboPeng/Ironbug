using OpenStudio;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.RhinoOpenStudio.GeometryConverter
{
    public static class OpenStudioExtension
    {
        public static Brep ToBrepFromSurface(this OpenStudio.Surface surface)
        {
            var b = surface.ToBrep();
            var userData = b.Surfaces[0].UserData.First(_ => _ is OsmObjectData) as OsmObjectData;

            userData.OsmObjProperties.Set("isPartOfEnvelope", surface.isPartOfEnvelope());
            userData.OsmObjProperties.Set("surfaceType", surface.surfaceType());

            //userData.OsmObjProperties.Set("dd", Mesh.CreateFromBrep(b)[0]);

            return b;
        }

        public static Brep ToBrep(this PlanarSurface planarSurface)
        {
            var pts = planarSurface.vertices().Select(pt => new Rhino.Geometry.Point3d(pt.x(), pt.y(), pt.z())).ToList();
            pts.Add(pts[0]);

            var crv = new PolylineCurve(pts);
            //crv.MakeClosed(tol); //Why does this is not working??? Rhino??

            var plannarBrep = Brep.CreatePlanarBreps(crv, 0.000001)[0];

            if (!plannarBrep.IsValid)
            {
                throw new System.Exception(string.Format("Failed to import {0}!", planarSurface.nameString()));
            }

            //add osm data as user data
            Rhino.Geometry.Surface srf = plannarBrep.Surfaces[0];
            var userData = new OsmObjectData();
            userData.Notes = planarSurface.__str__();
            srf.UserData.Add(userData);

            return plannarBrep;
        }

        public static IEnumerable<(string DataName, string DataValue, string DataUnit)> GetUserFriendlyFieldInfo(this IdfObject idfObject, bool ifIPUnits = false)
        {
            var com = idfObject;
            IddObject iddObject = com.iddObject();

            var valueWithInfo = new List<(string, string, string)>();
            var fieldCount = com.numFields();

            for (int i = 0; i < fieldCount; i++)
            {
                var ifIPUnit = ifIPUnits;

                uint index = (uint)i;
                var field = iddObject.getField(index).get();

                if (!field.IsWorkableField()) continue;
                var fieldProp = field.properties();
                var valueStr = com.getString(index).get();

                OSOptionalQuantity oQuantity = null;
                var customStr = String.Empty;
                if (field.IsRealType())
                {
                    //try to convert the unit and value
                    oQuantity = com.getQuantity(index, true, ifIPUnit);
                    customStr = oQuantity.isSet() ? oQuantity.get().value().ToString() : valueStr;
                }
                else
                {
                    customStr = valueStr;
                }

                var dataname = field.name();
                var defaultValue = GetDefaultValue(field, ifIPUnit);
                var unit = GetUnit(field, ifIPUnit);

                var shownValue = string.IsNullOrWhiteSpace(customStr) ? defaultValue : customStr;
                //shownValue = ReplaceGUIDString(shownValue);
                var shownUnit = string.IsNullOrWhiteSpace(unit) ? string.Empty : string.Format(" [{0}]", unit);
                var shownDefault = string.IsNullOrWhiteSpace(defaultValue) ? string.Empty : string.Format(" (Default: {0})", defaultValue);

                var att = String.Format("{0,-23} !- {1}{2}{3}", shownValue, dataname, shownUnit, shownDefault);

                valueWithInfo.Add((dataname, shownValue, shownUnit));
            }

            return valueWithInfo;

            string ReplaceGUIDString(string s)
            {
                var match = System.Text.RegularExpressions.Regex.Match(s, @"\{[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}\}",
       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                return match.Success ? "#[GUID]" : s;
            }

            string GetUnit(IddField field, bool ifIPUnit)
            {
                var optionalUnit = field.getUnits(ifIPUnit);
                var unit = string.Empty;
                if (optionalUnit.is_initialized())
                {
                    var unit2 = optionalUnit.get();
                    var prettyString = unit2.prettyString();
                    var standardString = unit2.standardString();

                    unit = string.IsNullOrWhiteSpace(prettyString) ? standardString : prettyString;
                }
                else
                {
                    unit = string.Empty;
                }

                return unit;
            }

            string GetDefaultValue(IddField field, bool ifIPUnit)
            {
                var sd = field.properties().stringDefault;
                var defaultStrStr = sd.isNull() ? string.Empty : sd.get();
                var defaultNumStr = GetDefaultNumStr(field, ifIPUnit);

                return string.IsNullOrWhiteSpace(defaultNumStr) ? defaultStrStr : defaultNumStr;
            }
            string GetDefaultNumStr(IddField field, bool ifIPUnit)
            {
                var numStr = String.Empty;
                var fieldProp = field.properties();
                if (fieldProp.numericDefault.is_initialized())
                {
                    var num = fieldProp.numericDefault.get();
                    //autosized
                    if (num == -9999) return numStr;

                    var si = fieldProp.units;
                    if (si.isNull()) return numStr;

                    if (!ifIPUnit) return num.ToString();

                    var siStr = si.get();

                    var ipStr = GetUnit(field, true);
                    if (string.IsNullOrWhiteSpace(ipStr)) return numStr;

                    var uconvert = OpenStudioUtilitiesUnits.convert(num, siStr, ipStr);

                    numStr = uconvert.is_initialized() ? uconvert.get().ToString() : "#SI" + num.ToString();
                }
                return numStr;
            }
        }

        public static bool IsWorkableField(this IddField iddField)
        {
            //https://bigladdersoftware.com/epx/docs/8-0/input-output-reference/page-004.html
            //!Type of data for the field -
            //!integer
            //!real
            //!alpha    (arbitrary string),
            //!choice   (alpha with specific list of choices, see
            //!                                 \key)
            //!object-list  (link to a list of objects defined elsewhere,
            //!             see \object - list and \ reference)
            //!external-list    (uses a special list from an external source,
            //!             see \external - list)
            //!node         (name used in connecting HVAC components)

            var dataType = iddField.properties().type.valueDescription();
            var name = iddField.name().ToLower();
            var result = dataType.ToLower() != "object-list" ? true : !name.Contains("node");
            result &= dataType != "node";
            result &= dataType != "external-list";
            result &= dataType != "handle";
            return result;
        }

        public static bool IsRealType(this IddField iddField)
        {
            return iddField.properties().type.valueDescription() == "real";
        }
    }
}