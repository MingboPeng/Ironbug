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
        int _processCount = 0;
        int _totalCount = 0;

        public IntersectMass() => Instance = this;

        public static IntersectMass Instance
        {
            get; private set;
        }
        
        public override string EnglishName => "IntersectMass";

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
                    checkedBrepObjs.Add(extrusion.ToBrep());
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
                    this._processCount = 0;
                    this._totalCount = oldBreps.Count;
                    System.Action action = () => RhinoApp.WriteLine("Processing {0}/{1} Breps.", this._processCount, this._totalCount);
                    var results = oldBreps.AsParallel().AsOrdered().Select(b => SplitBrepWithBreps(b, oldBreps, tolerance, ref _processCount, action));

                    var finishedCount = 0;
                    foreach (var newBrep in results)
                    {
                        //Use delete instead of replace for removing the userdata as well.
                        RhinoDoc.ActiveDoc.Objects.AddBrep(newBrep);
                        RhinoDoc.ActiveDoc.Objects.Delete(selectedObjs[finishedCount],true); 

                        //RhinoDoc.ActiveDoc.Objects.Replace(selectedObjs[finishedCount], newBrep);
                        finishedCount++;
                    }

                    RhinoApp.WriteLine("Done! Finished {0} Breps.", finishedCount);
                }
                catch (System.Exception ex)
                {

                    Rhino.UI.Dialogs.ShowMessageBox(ex.Message,"error");
                }
                
            }
            

            return Result.Success;
        }

        

        private static Brep SplitBrepWithBreps(Brep CurrentBrep, List<Brep> AllBreps, double tolerance, ref int processCount, System.Action PostAction)
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
            processCount++;
            PostAction();
            
            return currentBrep;
        }
    }
}