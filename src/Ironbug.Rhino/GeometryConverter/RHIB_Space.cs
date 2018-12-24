using Rhino.DocObjects.Custom;
using Rhino.Geometry;
using System.Collections.Generic;
using OPS = OpenStudio;
using System.Linq;
using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace Ironbug.RhinoOpenStudio.GeometryConverter
{
    public class RHIB_Space : CustomBrepObject
    {
        public RHIB_Space(Brep m)
            : base(m)
        {
        }

        public RHIB_Space()
        {
        }

        public override string ToString()
        {
            return "RHIB_Space";
        }

        public override string ShortDescription(bool plural)
        {
            return "RHIB_Space";
        }

        public string OSM_String = string.Empty;

        public static (RHIB_Space space, List<Brep> glzs) FromOpsSpace(OPS.Space OpenStudioSpace)
        {
            var ospace = OpenStudioSpace;
            var sfs = ospace.surfaces;
            var tol = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
            
            var zonefaces = new List<Brep>();
            var glzs = new List<Brep>();

            //var zoneBrep1 = new Brep();
            //var zoneBrep2 = new Brep();
            var zoneBrep3 = new Brep();
            foreach (OPS.Surface sf in sfs)
            {
                var surfaceBrep = sf.ToBrep();
                var glzSurfaceBrep = sf.subSurfaces().Select(s => s.ToBrep());
               
                zonefaces.Add(surfaceBrep);

                glzs.AddRange(glzSurfaceBrep);
                //test
                //zoneBrep1.Join(face.BrepGeometry, tol, true);
                //zoneBrep2.AddSurface(face.BrepGeometry.Surfaces[0]);
                
                zoneBrep3.Append(surfaceBrep);
                //zoneBrep3.Faces.Last().UserDictionary.ReplaceContentsWith(face.UserDictionary);
                //zoneBrep3.Faces.Last().UserData.Add(sfud);

            }
            //var isclosed1 = zoneBrep1.IsSolid;
            //var isclosed2 = zoneBrep2.IsSolid;
            var isclosed3 = zoneBrep3.IsSolid;

            //zoneBrep1.JoinNakedEdges(tol);
            //zoneBrep2.JoinNakedEdges(tol);
            zoneBrep3.JoinNakedEdges(tol);

            var closedBrep = Brep.JoinBreps(zonefaces, tol)[0];

            var userData = new OsmString();
            userData.Notes = ospace.__str__();
            closedBrep.UserData.Add(userData);
            

            var space = new RHIB_Space(closedBrep);
            space.Name = ospace.nameString();
            //space.UserDictionary.Set("OSM_String", ospace.__str__());
            //space.UserDictionary.Set("OSM_Object", ObjectToByteArray(ospace));
            //space.OSM_String = ospace.__str__();
            

            return (space, glzs);
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

    public static class OpenStudioExtension
    {
        public static Brep ToBrep(this OPS.PlanarSurface planarSurface)
        {
            var tol = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance; //TODO: move this to somewhere cleaner!!!
            var pts = planarSurface.vertices().Select(pt => new Point3d(pt.x(), pt.y(), pt.z())).ToList();
            pts.Add(pts[0]);
            //crv.MakeClosed(tol); //Why does this is not working??? Rhino??
            var crv = new PolylineCurve(pts);
            Surface srf = Brep.CreatePlanarBreps(crv, tol)[0].Surfaces[0];
            
            if (!srf.IsValid)
            {
                throw new System.Exception(string.Format("Failed to import {0}!", planarSurface.nameString()));
            }
            

            var userData = new OsmString();
            userData.Notes = planarSurface.__str__();
            srf.UserData.Add(userData);

            var brepWithSrf = srf.ToBrep();
            return brepWithSrf;
        }
        public static RHIB_Surface ToOpsSurface(this OPS.Surface surface)
        {
            var brep = surface.ToBrep();
            var s = new RHIB_Surface(brep);
            s.Name = surface.nameString();

            var userData = new OsmString();
            userData.Notes = surface.__str__();
            s.UserData.Add(userData);

            return s;
        }
    }
    
    public class RHIB_Surface : CustomBrepObject
    {
        public RHIB_Surface(Brep m)
            : base(m)
        {
        }

        public RHIB_Surface()
        {
        }

        public RHIB_Surface(OPS.Surface surface)
        {
            //var brep = surface.ToBrep();
            
            //var s = new RHIB_Surface(brep);
            //s.Name = surface.nameString();
            //s.UserDictionary.Set("OSM_String", surface.__str__());
            //return s;
            //this.sub
        }



    }





}