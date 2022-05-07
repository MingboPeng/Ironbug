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
            pManager.AddBrepParameter("Zones", "_zones", "objects to be picked", GH_ParamAccess.list);
            pManager[pManager.AddBrepParameter("Scopes", "scopes", "Scope breps for picking zone breps based on its centroid location.", GH_ParamAccess.list)].Optional = true;
            
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Selected Zones", "zones", "Picked objects", GH_ParamAccess.list);
            pManager.AddBrepParameter("Unselected Zones", "unSelected", "Unselected objects", GH_ParamAccess.list);
            (pManager[1] as GH.Kernel.Parameters.Param_Brep).Hidden = true;
        }

        List<GH_Brep> AllInputBreps = new List<GH_Brep>();
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var allBps = new List<GH_Brep>();
            if (!DA.GetDataList(0, allBps)) return;
            AllInputBreps = allBps;

            var nodes = new List<GH_Brep>();
            DA.GetDataList(1, nodes);

            var unselectedZones = new List<GH_Brep>();
            if (nodes.Count > 0)
            {
                var selectedZs = GetZoneFromNode(allBps, nodes, out unselectedZones);
                DA.SetDataList(0, selectedZs);
            }
            else
            {
                DA.SetDataList(0, allBps);
            }

            DA.SetDataList(1, unselectedZones);

        }

        private static List<GH_Brep> GetZoneFromNode(List<GH_Brep> allBps, IEnumerable<GH_Brep> outBx, out List<GH_Brep> Unselected)
        {
            var selectedZones = new List<GH_Brep>();
            var unselectedZones = new List<GH_Brep>();
            

            var inputBrpsCenterPts = allBps.AsParallel().AsOrdered().Select(_ => VolumeMassProperties.Compute(_.Value).Centroid);
            var num = 0;
            foreach (var pt in inputBrpsCenterPts)
            {
                var isSel = outBx.AsParallel().FirstOrDefault(_ =>  _.Value.IsPointInside(pt,0.0001,true)) != null;
                var currentItem = allBps[num];
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

            if (!allBps.Any())
            {
                return;
            }
            var zParm = this.Params.Input[0] as GH.Kernel.Parameters.Param_Brep;
            zParm.Hidden = false;
            var nodeIds = new List<Guid>();
            var centers = allBps.AsParallel().Select(_ => VolumeMassProperties.Compute(_.Value).Centroid);

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