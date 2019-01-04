using Rhino.DocObjects.Custom;
using Rhino.Geometry;
using OPS = OpenStudio;

namespace Ironbug.RhinoOpenStudio.GeometryConverter
{
    public class RHIB_SubSurface : CustomBrepObject, IRHIB_GeometryBase
    {
        //IDictionary<string, string> m_OpenStudioProperties;

        public RHIB_SubSurface(Brep m)
            : base(m)
        {
        }

        public RHIB_SubSurface()
        {
        }

        public static RHIB_SubSurface ToRHIB_SubSurface(Brep brep, string IdfString)
        {
            var srf = new RHIB_SubSurface(brep);
            var userDataDic = new Rhino.Collections.ArchivableDictionary();
            userDataDic.Set("SubSurfaceData", IdfString);

            srf.Attributes.UserDictionary.Set("OpenStudioData", userDataDic);

            return srf;
        }

        private Rhino.Collections.ArchivableDictionary GetIdfData() => this.Attributes.UserDictionary.GetDictionary("OpenStudioData");

        public string GetIdfString() => this.GetIdfData().GetString("SubSurfaceData");

        public override string ShortDescription(bool plural) => "OS_SubSurface";

        public bool ToOS(OPS.Model model)
        {
            var rhBrep = this.BrepGeometry;
            var rhVts = rhBrep.Vertices;
            var osmStr = rhBrep.Surfaces[0].UserData.Find(typeof(OsmObjectData)) as OsmObjectData;
            var osmIdfobj = OpenStudio.IdfObject.load(osmStr.IDFString).get();

            var handle = osmIdfobj.handle();
            var osmObj = model.getSubSurface(handle).get();

            var osmVets = new OPS.Point3dVector();
            foreach (var pt in rhVts)
            {
                var p = pt.Location;
                osmVets.Add(new OPS.Point3d(p.X, p.Y, p.Z));
            }

            return osmObj.setVertices(osmVets);
        }

        public bool Update()
        {
            //1) edit idf string only,
            //2) check the new idf string in a dummy OpenStudio.Model, and match their idf string

            var result = false;
            var m = IronbugRhinoPlugIn.Instance.OsmModel;
            var rhBrep = this.BrepGeometry;

            var rhVts = rhBrep.Vertices;

            var osmData = this.GetOsmObjectData();
            var osmIdfobj = OpenStudio.IdfObject.load(osmData.IDFString).get();
            var handle = osmIdfobj.handle();

            var osmObj = m.getSubSurface(handle).get();

            var osmVets = new OPS.Point3dVector();
            foreach (var pt in rhVts)
            {
                var p = pt.Location;
                osmVets.Add(new OPS.Point3d(p.X, p.Y, p.Z));
            }

            result = osmObj.setVertices(osmVets);
            result &= osmObj.setName(this.Name).is_initialized();

            if (result)
            {
                //update the osm strings attached to rhino geometry
                osmData.IDFString = osmObj.__str__();
                return true;
            }

            return false;
        }

        public bool UpdateIdfData(int IddFieldIndex, string Value, string BrepFaceCenterAreaID = "")
        {
            var idfString = this.GetIdfString();

            //Update IdfString
            var osmIdfobj = OpenStudio.IdfObject.load(idfString).get();
            osmIdfobj.setString((uint)IddFieldIndex, Value);
            var newIdfString = osmIdfobj.__str__();

            if (!newIdfString.Contains(Value))
                return false; //TODO: add exception message
            
            this.GetIdfData().Remove("SubSurfaceData");
            this.GetIdfData().Set("SubSurfaceData", newIdfString);

            return true;
        }

        public RHIB_SubSurface Duplicate()
        {
            var newObj = new RHIB_SubSurface(this.DuplicateBrepGeometry());
            return newObj;
        }
    }

    public sealed class SubSurface_FeildSet
    {
        //
    }
}