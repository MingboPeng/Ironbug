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
          : base("Ironbug_ZonePicker", "ZonePicker",
              "Description",
              "Ironbug", "00:Ironbug")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Zones", "_zones", "objects to be picked", GH_ParamAccess.list);
            pManager[pManager.AddBoxParameter("Scopes", "scopes", "Scope boxes for picking zone breps.", GH_ParamAccess.list)].Optional = true;
            
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Selected Zones", "zones", "Picked objects", GH_ParamAccess.list);
            pManager.AddBrepParameter("Unselected Zones", "unselected", "Unselected objects", GH_ParamAccess.list);
            (pManager[1] as GH.Kernel.Parameters.Param_Brep).Hidden = true;
        }

        List<GH_Brep> AllInputBreps = new List<GH_Brep>();
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var allBps = new List<GH_Brep>();
            if (!DA.GetDataList(0, allBps)) return;
            AllInputBreps = allBps;

            var nodes = new List<GH_Box>();
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

        private static List<GH_Brep> GetZoneFromNode(List<GH_Brep> allBps, IEnumerable<GH_Box> outBx, out List<GH_Brep> Unselected)
        {
            var selectedZones = new List<GH_Brep>();
            var unselectedZones = new List<GH_Brep>();
            foreach (var item in allBps)
            {

                var sBp = item.DuplicateBrep();
               
                var isSelected = false;
                foreach (var b in outBx)
                {
                    var ct = VolumeMassProperties.Compute(sBp.Value);
                    isSelected = b.Value.Contains(ct.Centroid);
                    if (isSelected)
                    {
                        selectedZones.Add(item);
                        break;
                    }
                }

                if (!isSelected)
                {
                    unselectedZones.Add(item);
                }
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

            if (allBps.Count==0)
            {
                return;
            }
            var zParm = this.Params.Input[0] as GH.Kernel.Parameters.Param_Brep;
            zParm.Hidden = false;
            var nodeIds = new List<Guid>();
            foreach (var item in allBps)
            {
                var cc = VolumeMassProperties.Compute(item.Value);
                //var cc = item.Value.GetBoundingBox(false).Center;
                var bbox = GenZoneNode(cc.Centroid);
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
                doc.Views.Redraw();
                return;
            }

            //var outBx2 = new List<GH_Brep>();

            var scopeParm = this.Params.Input[1] as GH.Kernel.Parameters.Param_Box;
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
            var nds = pickedZoneNodes.Select(_ => new GH_Box(GenZoneNode(_.Boundingbox.Center)));


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