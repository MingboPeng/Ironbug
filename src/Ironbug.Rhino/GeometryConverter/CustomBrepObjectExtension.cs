using OpenStudio;
using Rhino.DocObjects.Custom;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.RhinoOpenStudio.GeometryConverter
{
    public static class CustomBrepObjectExtension
    {
        public static OsmObjectData GetOsmObjectData(this CustomBrepObject brep)
        {
            OsmObjectData osmObject = null;
            var isSrf = brep.BrepGeometry.IsSurface;
            if (isSrf)
            {
                osmObject = brep.BrepGeometry.Surfaces[0].UserData.Find(typeof(OsmObjectData)) as OsmObjectData;
            }
            else
            {
                osmObject = brep.BrepGeometry.UserData.Find(typeof(OsmObjectData)) as OsmObjectData;
            }
            
            //TODO: this shouldn't return null case.....
            return osmObject;
        }

        public static OsmObjectData GetOsmObjectData(this Brep brep)
        {
            OsmObjectData osmObject = null;
            var isSrf = brep.IsSurface;
            if (isSrf)
            {
                osmObject = brep.Surfaces[0].UserData.Find(typeof(OsmObjectData)) as OsmObjectData;
            }
            else
            {
                osmObject = brep.UserData.Find(typeof(OsmObjectData)) as OsmObjectData;
            }

            //TODO: this shouldn't return null case.....
            return osmObject;
        }

        public static string GetCentorAreaForID(this BrepFace SurfaceAsBrep)
        {
            var str = string.Empty;
            if (SurfaceAsBrep.IsSurface)
            {
                var face = SurfaceAsBrep;
                var prop = AreaMassProperties.Compute(SurfaceAsBrep);
                var c = prop.Centroid;
                var a = prop.Area;

                str = string.Format("{0}_{1}_{2}_{3}", c.X, c.Y, c.Z, a);
            }

            return str;
        }

        public static string GetCentorAreaForID(this Brep SurfaceAsBrep)
        {
            var str = string.Empty;
            if (SurfaceAsBrep.IsSurface)
            {
                var face = SurfaceAsBrep.Faces[0];
                str = face.GetCentorAreaForID();
            }

            return str;
        }

    }
}