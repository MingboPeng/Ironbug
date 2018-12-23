﻿using Rhino.DocObjects.Custom;
using Rhino.Geometry;
using System.Collections.Generic;
using OPS = OpenStudio;
using System.Linq;

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
                var face = sf.ToBrep();
                var subsrfs = sf.subSurfaces().Select(s => s.ToBrep());
                zonefaces.Add(face);
                glzs.AddRange(subsrfs);

                //test
                //zoneBrep1.Join(face.BrepGeometry, tol, true);
                //zoneBrep2.AddSurface(face.BrepGeometry.Surfaces[0]);
                zoneBrep3.Append(face);
                zoneBrep3.Faces.Last().UserDictionary.ReplaceContentsWith(face.UserDictionary);
            }
            //var isclosed1 = zoneBrep1.IsSolid;
            //var isclosed2 = zoneBrep2.IsSolid;
            var isclosed3 = zoneBrep3.IsSolid;

            //zoneBrep1.JoinNakedEdges(tol);
            //zoneBrep2.JoinNakedEdges(tol);
            zoneBrep3.JoinNakedEdges(tol);

            var b = Brep.JoinBreps(zonefaces, tol)[0];
            var space = new RHIB_Space(b);
            
            space.Name = ospace.nameString();
            space.UserDictionary.Set("OSM_String", ospace.__str__());
            //space.OSM_String = ospace.__str__();

            return (space, glzs);
        }

        
        
        
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
            var srf = Brep.CreatePlanarBreps(crv, tol)[0];

            if (!srf.IsValid)
            {
                throw new System.Exception(string.Format("Failed to import {0}!", planarSurface.nameString()));
            }
            
            srf.Faces[0].UserDictionary.Set("OSM_String", planarSurface.__str__());
            return srf;
        }
        public static RHIB_Surface ToOpsSurface(this OPS.Surface surface)
        {
            var brep = surface.ToBrep();
            var s = new RHIB_Surface(brep);
            s.Name = surface.nameString();
            s.UserDictionary.Set("OSM_String", surface.__str__());

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