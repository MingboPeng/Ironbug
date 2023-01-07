using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_VizCurve : Ironbug_Component
    {
        
        public Ironbug_VizCurve()
          : base("IB_VizCurve", "VizCurve",
              "Visualize performance curve",
              "Ironbug", "07:Curve & Load")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Curve", "_curve", "Curve to visualize", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Geometry", "Geometry", "Geometry", GH_ParamAccess.list);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            HVAC.BaseClass.IB_Curve cv = null;
            DA.GetData(0, ref cv);

            var pts = new List<Rhino.Geometry.Point3d>();
            var size = 10;
            if (cv is HVAC.IIB_Curve3D bc)
            {
                bc.GetMinMax(out var minX, out var maxX, out var minY, out var MaxY);
                pts = GenPts(minX, maxX, minY, MaxY, size, (x, y) => bc.Compute(x, y));
                var srf = NurbsSurface.CreateFromPoints(pts, size, size, 5, 5);
                DA.SetData(0, srf);
            }
            else if (cv is HVAC.IIB_Curve2D ln)
            {
                ln.GetMinMax(out var minX, out var maxX);
                pts = GenPts(minX, maxX, size, (x) => ln.Compute(x));
                var geo = Curve.CreateInterpolatedCurve(pts, 5);
                DA.SetData(0, geo);
            }
            else
            {
                throw new ArgumentException("Cannot visualize CurveFanPressureRise");
            }
           

        }


        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("D88F968A-37F0-427B-A0AB-84E5D6DBDF1F");

        public static List<double> GenNumList(double start, double end, int count)
        {
            var r = end - start;
            var step = r/ count;
            return Enumerable.Range(1, count).Select(_ => start + _ * step).ToList();
        }

        private static List<Point3d> GenPts(double minX, double maxX, double minY, double MaxY, int count, Func<double, double, double> cal)
        {
            var pts = new List<Rhino.Geometry.Point3d>();
            var xs = GenNumList(minX, maxX, count);
            var ys = GenNumList(minY, MaxY, count);

            for (int i = 0; i < count; i++)
            {
                var x = xs[i];
                for (int j = 0; j < count; j++)
                {
                    var y = ys[j];
                    var z = cal(x, y);
                    var pt = new Rhino.Geometry.Point3d(x, y, z);
                    pts.Add(pt);
                }
            }
            return pts;
        }

        private static List<Point3d> GenPts(double minX, double maxX, int count, Func<double, double> cal)
        {
            var xs = GenNumList(minX, maxX, count);
            var pts = xs.Select(_ => new Rhino.Geometry.Point3d(_, 0, cal(_))).ToList();
            return pts;
        }

    }
}