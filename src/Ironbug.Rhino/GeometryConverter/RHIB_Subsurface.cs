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
        public RHIB_SubSurface(Brep m)
            : base(m)
        {
        }

        public RHIB_SubSurface()
        {
        }

        public RHIB_SubSurface(OPS.SubSurface surface) : this(surface.ToBrep())
        {
            this.Name = surface.nameString();
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


    }
}
