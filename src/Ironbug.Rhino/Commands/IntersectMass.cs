using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input.Custom;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.RhinoOpenStudio.Commands
{
    public class IntersectMass : Command
    {
        public IntersectMass()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static IntersectMass Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "IntersectMass"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var tolerance = Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance / 10;

            var go = new GetObject();
           
            go.SetCommandPrompt("Select closed (solid) Breps or Boxs to intersect");
            go.GeometryFilter = ObjectType.AnyObject;
            go.GetMultiple(2, -1);

            if (go.CommandResult() != Result.Success)
                return go.CommandResult();


            var selectedObjs = go.Objects();
            var checkedBrepObjs = new List<Brep>();
            foreach (var item in selectedObjs)
            {
                var geo = item.Geometry();
                if (geo is Extrusion extrusion)
                {
                    checkedBrepObjs.Add(extrusion.ToBrep().DuplicateBrep());
                }
                else if (geo is Brep brep)
                {
                    checkedBrepObjs.Add(brep.DuplicateBrep());
                }
            }

            List<Brep> oldBreps = checkedBrepObjs;

            if (oldBreps.Count() > 0)
            {
                try
                {
                    //var results = oldBreps.AsParallel().AsOrdered().Select(b => SplitBrepWithBreps(b, oldBreps, tolerance));
                    var results = oldBreps.Select(b => SplitBrepWithBreps(b, oldBreps, tolerance));
                    int count = 0;

                    //while (results.Count()< oldBreps.Count())
                    //{
                    //    RhinoApp.WriteLine("Finished {0}/{1}", results.Count() + 1, oldBreps.Count);
                    //}
                    foreach (var newBrep in results)
                    {
                        //RhinoDoc.ActiveDoc.Objects.AddBrep(newBrep);
                        //RhinoDoc.ActiveDoc.Objects.Delete(selectedObjs[count],false);
                        RhinoDoc.ActiveDoc.Objects.Replace(selectedObjs[count], newBrep);
                        RhinoApp.WriteLine("Finished {0}/{1}", count + 1, oldBreps.Count);
                        count++;
                    }
                }
                catch (System.Exception ex)
                {

                    Rhino.UI.Dialogs.ShowMessage(ex.Message,"error");
                }
                

                
            }

            

            return Result.Success;
        }

        private static Brep SplitBrepWithBreps(Brep CurrentBrep, List<Brep> AllBreps, double tolerance)
        {
            var currentBrep = CurrentBrep.DuplicateBrep();
            var allBreps = AllBreps;

            foreach (Brep item in allBreps)
            {
                var tempBrep = currentBrep.Split(item, tolerance);
                if (tempBrep.Count() > 0)
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