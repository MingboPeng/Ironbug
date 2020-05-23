using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.LBHB_Legacy
{
    public class Honeybee_IntersectMassII : GH_Component
    {
        public Honeybee_IntersectMassII()
            :base("Honeybee_IntersectMassII","IntersectMass", "Intersect closed Breps with adjacent Breps a couple times faster than old Honeybee_IntersectMass", "HB-Legacy", "00 | Honeybee")
        {
        }
        public override Guid ComponentGuid => new Guid("25A370F2-FB33-422D-89A7-CA81FFE8BF7D");
        protected override System.Drawing.Bitmap Icon => LBHB_Legacy.Properties.Resources.IntersectMass;
        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Breps", "Breps_", "Breps to be intersected", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Breps", "Breps", "Breps have been intersected", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            var tolerance = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
            var allOldBreps = new List<Brep>(); DA.GetDataList(0, allOldBreps);

            if (allOldBreps.Any())
            {
                var results = allOldBreps.AsParallel().AsOrdered().Select(b => SplitBrepWithBreps(b, allOldBreps, tolerance));
                DA.SetDataList(0, results);
            }
        }

        static Brep SplitBrepWithBreps(Brep CurrentBrep, List<Brep> AllBreps, double tolerance)
        {
            var currentBrep = CurrentBrep;
            var allBreps = AllBreps;

            foreach (Brep item in allBreps)
            {
                var tempBrep = currentBrep.Split(item, tolerance);
                if (tempBrep.Any())
                {
                    var newBrep = Brep.JoinBreps(tempBrep, tolerance);

                    currentBrep = newBrep.First();
                    currentBrep.Faces.ShrinkFaces();
                }
            }
            return currentBrep;

        }
    }
}
