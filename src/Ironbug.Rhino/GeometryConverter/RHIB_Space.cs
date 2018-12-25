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
            return "Space";
        }

        public override string ShortDescription(bool plural) => "Space";

        public string OSM_String = string.Empty;

        public static (RHIB_Space space, List<RHIB_SubSurface> glzs) FromOpsSpace(OPS.Space OpenStudioSpace)
        {
            var ospace = OpenStudioSpace;
            var sfs = ospace.surfaces;
            var tol = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
            //tol = 0.000001;

            var zonefaces = new List<Brep>();
            var glzs = new List<RHIB_SubSurface>();
            var zoneBrep3 = new Brep();

            
            foreach (OPS.Surface sf in sfs)
            {
                var surfaceBrep = sf.ToBrep();
                var glzSurfaceBrep = sf.subSurfaces().Select(s => new RHIB_SubSurface(s));
               
                zonefaces.Add(surfaceBrep);
                zoneBrep3.Append(surfaceBrep);

                glzs.AddRange(glzSurfaceBrep);
                
            }

            zoneBrep3.JoinNakedEdges(tol);
            var closedBrep = Brep.JoinBreps(zonefaces, tol)[0];
            if (!closedBrep.IsSolid)
            {
                closedBrep = zoneBrep3;
            }

            //add osm info to user data
            var userData = new OsmString();
            userData.Notes = ospace.__str__();
            closedBrep.UserData.Add(userData);
            

            var space = new RHIB_Space(closedBrep);
            space.Name = ospace.nameString();
            
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

            var pts = planarSurface.vertices().Select(pt => new Point3d(pt.x(), pt.y(), pt.z())).ToList();
            pts.Add(pts[0]);
            
            var crv = new PolylineCurve(pts);
            //crv.MakeClosed(tol); //Why does this is not working??? Rhino??

            var plannarBrep = Brep.CreatePlanarBreps(crv, 0.000001)[0];

            if (!plannarBrep.IsValid)
            {
                throw new System.Exception(string.Format("Failed to import {0}!", planarSurface.nameString()));
            }
            
            //add osm data as user data
            Surface srf = plannarBrep.Surfaces[0];
            var userData = new OsmString();
            userData.Notes = planarSurface.__str__();
            srf.UserData.Add(userData);
            
            return plannarBrep;
        }
       
    }
    
    public class RHIB_SubSurface : CustomBrepObject
    {
        public RHIB_SubSurface(Brep m)
            : base(m)
        {
        }

        public RHIB_SubSurface()
        {
        }

        public RHIB_SubSurface(OPS.SubSurface surface):this (surface.ToBrep())
        {
            this.Name = surface.nameString();
        }

        public override string ShortDescription(bool plural) => "SubSurface";

    }





}