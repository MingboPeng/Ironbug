using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using GH = Grasshopper;
using System;
using System.Collections.Generic;
using System.Linq;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZonePicker : GH_Component
    {
        protected override System.Drawing.Bitmap Icon => Properties.Resources.ZonePicker;

        public override Guid ComponentGuid => new Guid("{C7B8CDA1-4C8C-4236-A7CF-C573A2258984}");
        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        public Ironbug_ZonePicker()
          : base("IB_ZonePicker", "ZonePicker",
              "Description",
              "Ironbug", "00:Ironbug")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Zones", "_zones", "objects to be picked", GH_ParamAccess.list);
            pManager[pManager.AddBrepParameter("Scopes", "scopes", "Scope breps for picking zone breps based on its centroid location.", GH_ParamAccess.list)].Optional = true;
            
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Selected Zones", "zones", "Picked objects", GH_ParamAccess.list);
            pManager.AddGenericParameter("Unselected Zones", "unSelected", "Unselected objects", GH_ParamAccess.list);
        }

        List<(object room, Point3d centroid)> AllInputBreps = new List<(object, Point3d)>();
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var allBps = new List<object>();
            if (!DA.GetDataList(0, allBps)) return;

            var cs = allBps.Select(_=> ( room : _, centroid : Helper.GetRoomCenter(_) )).ToList();
            AllInputBreps = cs;

            var nodes = new List<GH_Brep>();
            DA.GetDataList(1, nodes);

            var unselectedZones = new List<object>();
            if (nodes.Count > 0)
            {
                var selectedZs = GetZoneFromNode(cs, nodes, out unselectedZones);
                DA.SetDataList(0, selectedZs);
            }
            else
            {
                DA.SetDataList(0, allBps);
            }

            DA.SetDataList(1, unselectedZones);

        }

        private static List<object> GetZoneFromNode(List<(object room, Point3d centroid)> allBps, IEnumerable<GH_Brep> outBx, out List<object> Unselected)
        {
            var selectedZones = new List<object>();
            var unselectedZones = new List<object>();
            

            var num = 0;
            foreach (var rmC in allBps)
            {
                var c = rmC.centroid;
                if (c == Point3d.Unset)
                    continue;

                var isSel = outBx.AsParallel().FirstOrDefault(_ =>  _.Value.IsPointInside(c,0.0001,true)) != null;
                var currentItem = allBps[num].room;
                if (isSel)
                {
                    selectedZones.Add(currentItem);
                }
                else
                {
                    unselectedZones.Add(currentItem);
                }
                num++;
            }

            Unselected = unselectedZones;
            return selectedZones;
        }

        public override void CreateAttributes()
        {
            var att = new IB_ComponentButtonAttributes(this);
            att.ButtonText = "Pick ZNodes";
            att.MouseDownEvent += (object obj) => this.PickZones();
            this.Attributes = att;
        }

        private void PickZones()
        {
            var doc = Rhino.RhinoDoc.ActiveDoc;
            var att = new Rhino.DocObjects.ObjectAttributes();
            
            var allBps = this.AllInputBreps;

            if (!allBps.Any()) return;

            var nodeIds = new List<Guid>();
            var centers = allBps.Select(_ => _.centroid);

            foreach (var item in centers)
            {
                var bbox = GenZoneNode(item);
                nodeIds.Add(doc.Objects.AddBrep(bbox.ToBrep()));
            }
            doc.Objects.UnselectAll();
            doc.Views.Redraw();

            GH.Instances.DocumentEditor.FadeOut();
            //GH.Instances.DocumentEditor.lo
            var pickedZoneNodes = GH.Getters.GH_BrepGetter.GetBreps();
            GH.Instances.DocumentEditor.FadeIn();
           
            if (pickedZoneNodes == null || pickedZoneNodes.Count==0)
            {
                doc.Objects.Delete(nodeIds, true);
                doc.Views.Redraw();
                return;
            }

            //var outBx2 = new List<GH_Brep>();

            var scopeParm = this.Params.Input[1] as GH.Kernel.Parameters.Param_Brep;
            scopeParm.Hidden = true;
   
            scopeParm.RemoveAllSources();
            scopeParm.ClearData();
            scopeParm.PersistentData.Clear();

            foreach (var item in pickedZoneNodes)
            {
                item.LoadGeometry(doc);
            }
            doc.Objects.Delete(nodeIds, true);
          
            //var nds = pickedZoneNodes.Select(_ => new GH_Brep( GenZoneNode(_.Boundingbox.Center)));
            var nds = pickedZoneNodes.Select(_ => new GH_Brep(GenZoneNode(_.Boundingbox.Center).ToBrep()));

            scopeParm.SetPersistentData(nds);
            ExpireSolution(true);

        }

        private Rhino.Geometry.BoundingBox GenZoneNode(Rhino.Geometry.Point3d LocPt)
        {
            var sz = 0.5;

            Rhino.Geometry.Point3d pt0 = new Rhino.Geometry.Point3d(-sz, -sz, -sz);
            Rhino.Geometry.Point3d pt1 = new Rhino.Geometry.Point3d(sz, sz, sz);

            Rhino.Geometry.BoundingBox box = new Rhino.Geometry.BoundingBox(pt0, pt1);
            var trans = Rhino.Geometry.Transform.Translation(new Rhino.Geometry.Vector3d(LocPt));
            box.Transform(trans);
            
            return box;
        }

    }
}