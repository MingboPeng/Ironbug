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
    
    }
}