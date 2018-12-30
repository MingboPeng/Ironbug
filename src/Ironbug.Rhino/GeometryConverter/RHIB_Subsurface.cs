using Rhino.DocObjects.Custom;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPS = OpenStudio;

namespace Ironbug.RhinoOpenStudio.GeometryConverter
{
    public class RHIB_SubSurface : CustomBrepObject
    {
        //IDictionary<string, string> m_OpenStudioProperties;

        public RHIB_SubSurface(Brep m)
            : base(m)
        {
            
        }

        public RHIB_SubSurface()
        {
        }

        public RHIB_SubSurface(OPS.SubSurface subSurface) : this(subSurface.ToBrep())
        {
            this.Name = subSurface.nameString();

            //var osmData = this.GetOsmObjectData();
            //if (osmData != null)
            //{

            //    subSurface.surface
            //    //TODO: need to create a helper to convert osm data string to OsmObjProperties dictionary items
            //    osmData.OsmObjProperties.Set()
            //}
        }

        public override string ShortDescription(bool plural) => "OS_SubSurface";

        public bool ToOS(OPS.Model model)
        {
            var rhBrep = this.BrepGeometry;
            var rhVts = rhBrep.Vertices;
            var osmStr = rhBrep.Surfaces[0].UserData.Find(typeof(OsmObjectData)) as OsmObjectData;
            var osmIdfobj = OpenStudio.IdfObject.load(osmStr.Notes).get();

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
            var osmIdfobj = OpenStudio.IdfObject.load(osmData.Notes).get();
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
                osmData.Notes = osmObj.__str__();
                return true;
            }

            return false;
        }

        public bool UpdateIdfString(int IddFieldIndex, string Value)
        {
            var m = IronbugRhinoPlugIn.Instance.OsmModel;
            var rhBrep = this.BrepGeometry;
            
            var osmData = this.GetOsmObjectData();
            var osmIdfobj = OpenStudio.IdfObject.load(osmData.Notes).get();

            osmIdfobj.setString((uint)IddFieldIndex, Value);

            var newIdfString = osmIdfobj.__str__();

            //TODO: also need to check the obj if works in osm. 
            //var handle = osmIdfobj.handle();
            //var newOsmObjString = m.getSubSurface(handle).get().__str__();

            //if (newIdfString == newOsmObjString)
            //{
            //    osmData.Notes = newIdfString;
            //    return true;
            //}

            if (newIdfString.Contains(Value))
            {
                osmData.Notes = newIdfString;
                return true;
            }

            
            return false;
            
        }


    }

    public sealed class SubSurface_FeildSet
    {
        //
    }
}
