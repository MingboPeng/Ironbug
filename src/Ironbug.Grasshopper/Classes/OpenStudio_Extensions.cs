using Ironbug.Grasshopper.Types;
using Rhino.Geometry;
using System.Collections.Generic;
using System.Linq;
using OPS = OpenStudio;

namespace Ironbug.Grasshopper
{
    public static class Model_Extensions
    {
        public static OPS.Model LoadOpsModel(string OsmPath)
        {
            var p = OpenStudio.OpenStudioUtilitiesCore.toPath(OsmPath);

            var trans = new OpenStudio.VersionTranslator();
            trans.setAllowNewerVersions(false);
            var tempModel = trans.loadModel(p);
            if (!tempModel.is_initialized())
            {
                throw new System.ArgumentException("Failed to open this file, and I don't know why!");
            }

            return tempModel.get();
        }
        public static Brep ToBrep(this OPS.PlanarSurface planarSurface)
        {
            var pts = planarSurface.vertices().Select(pt => new Rhino.Geometry.Point3d(pt.x(), pt.y(), pt.z())).ToList();
            pts.Add(pts[0]);

            var crv = new PolylineCurve(pts);

            var plannarBrep = Brep.CreatePlanarBreps(crv)[0];

            if (!plannarBrep.IsValid)
            {
                throw new System.Exception(string.Format("Failed to import {0}!", planarSurface.nameString()));
            }
            else
            {

                Rhino.Geometry.Surface srf = plannarBrep.Surfaces[0];
            }



            return plannarBrep;
        }

        public static OsZone ToOsZone(this OPS.ThermalZone thermalZone , out List<Brep> Glazings)
        {
            var spaces = thermalZone.spaces();
            var spacesBreps = new List<Brep>();
            var glzings = new List<Brep>();
            foreach (var sp in spaces)
            {
                sp.ToOsZone(out Brep zone, out List<Brep> glzs);
                spacesBreps.Add(zone);
                glzings.AddRange(glzs);

            }
            var mergedBrep = Brep.CreateBooleanUnion(spacesBreps, 10e-6)[0];

            Glazings = glzings;
           
            return new OsZone(mergedBrep, thermalZone.nameString());
        }

        public static bool ToOsZone(this OPS.Space OpenStudioSpace, out Brep Zone, out List<Brep> Glazings)
        {
            var ospace = OpenStudioSpace;
            var sfs = ospace.surfaces;
            var tol = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;

            var zonefaces = new List<Brep>();
            var glzs = new List<Brep>();
            var zoneBrep3 = new Brep();

            foreach (OPS.Surface sf in sfs)
            {
                //Surface
                var srf = sf.ToBrep();
                zonefaces.Add(srf);
                zoneBrep3.Append(srf);

                //SubSurface (Glasses)
                var winSrfs = sf.subSurfaces().Select(s => s.ToBrep());
                glzs.AddRange(winSrfs);

            }

            //Space
            zoneBrep3.JoinNakedEdges(tol);
            var closedBrep = Brep.JoinBreps(zonefaces, tol)[0];
            if (!closedBrep.IsSolid)
            {
                closedBrep = zoneBrep3;
            }

            var name = ospace.nameString();

            Zone = closedBrep;
            Glazings = glzs;
            return true;
        }

    }
}
