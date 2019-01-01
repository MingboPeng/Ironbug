using Rhino.DocObjects.Custom;
using Rhino.Geometry;
using System.Collections.Generic;
using OpenStudio;
using System.Linq;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

namespace Ironbug.RhinoOpenStudio.GeometryConverter
{
    public class RHIB_Space : CustomBrepObject,IRHIB_GeometryBase
    {
        public RHIB_Space(Brep m)
            : base(m)
        {
        }

        public RHIB_Space()
        {
        }


        public override string ToString() => "OS_Space";

        public override string ShortDescription(bool plural) => "OS_Space";

        public string OSM_String = string.Empty;

        public static (RHIB_Space space, List<RHIB_SubSurface> glzs) FromOpsSpace(Space OpenStudioSpace)
        {
            var ospace = OpenStudioSpace;
            var sfs = ospace.surfaces;
            var tol = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
            //tol = 0.000001;
            
            var zonefaces = new List<Brep>();
            var glzs = new List<RHIB_SubSurface>();
            var zoneBrep3 = new Brep();
            
          
            var userDataDic = new Rhino.Collections.ArchivableDictionary();
            userDataDic.Set("OpenStudioSpaceData", ospace.__str__());

            foreach (OpenStudio.Surface sf in sfs)
            {
                var rhBrep = sf.ToBrep();
                //surfaceBrep.Surfaces
                //var glzSurfaceBrep = sf.subSurfaces().Select(s => new RHIB_SubSurface(s));
               
                zonefaces.Add(rhBrep.SrfBrep);
                zoneBrep3.Append(rhBrep.SrfBrep);


                userDataDic.Set(rhBrep.SrfBrep.GetCentorAreaForID(), sf.__str__());
                
                //glzs.AddRange(glzSurfaceBrep);

            }

            zoneBrep3.JoinNakedEdges(tol);
            var closedBrep = Brep.JoinBreps(zonefaces, tol)[0];
            if (!closedBrep.IsSolid)
            {
                closedBrep = zoneBrep3;
            }

            //add osm info to user data
            //var userData = new OsmObjectData();

            //userData.IDFString = ospace.__str__();
            //closedBrep.UserData.Add(userData);
            
            //var att = new Rhino.DocObjects.ObjectAttributes();
            //att.UserData.Add(userData);
            

            var space = new RHIB_Space(closedBrep);

            space.Attributes.UserDictionary.AddContentsFrom(userDataDic);
            space.Name = ospace.nameString();
            
            return (space, glzs);

            
        }

        public bool UpdateIdfString(int IddFieldIndex, string Value)
        {

            var osmData = this.GetOsmObjectData();
            var osmIdfobj = OpenStudio.IdfObject.load(osmData.IDFString).get();

            osmIdfobj.setString((uint)IddFieldIndex, Value);
            var newIdfString = osmIdfobj.__str__();
            

            if (newIdfString.Contains(Value))
            {
                osmData.IDFString = newIdfString;
                return true;
            }
            
            return false;
        }

        //public static byte[] ObjectToByteArray(Object obj)
        //{
        //    BinaryFormatter bf = new BinaryFormatter();
        //    using (var ms = new System.IO.MemoryStream())
        //    {
        //        bf.Serialize(ms, obj);
        //        return ms.ToArray();
        //    }
        //}
        

    }

    //public class RHIB_Surface: BrepFace
    //{
    //    private RHIB_Surface()
    //    {
    //        this = base.CreateExtrusion()
    //    }
    //}

    

}