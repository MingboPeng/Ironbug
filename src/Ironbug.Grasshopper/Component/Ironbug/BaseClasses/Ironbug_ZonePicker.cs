using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Ironbug.HVAC.BaseClass;
using GH = Grasshopper;
using System;
using System.Collections.Generic;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZonePicker : Ironbug_Component
    {
        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("{C7B8CDA1-4C8C-4236-A7CF-C573A2258984}");
        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        public Ironbug_ZonePicker()
          : base("Ironbug_ZonePicker(WIP)", "ZonePicker",
              "Description",
              "Ironbug", "00:Ironbug")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Zones", "_zones", "objects to be picked", GH_ParamAccess.list);
            pManager[pManager.AddBrepParameter("Scopes", "scopes", "Scope objects for picking", GH_ParamAccess.list)].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Zones", "zones", "Picked objects", GH_ParamAccess.list);
        }

        List<GH_Brep> AllInputBreps = new List<GH_Brep>();
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var allBps = new List<GH_Brep>();
            DA.GetDataList(0, allBps);
            AllInputBreps = allBps;

            var nodes = new List<GH_Brep>();
            DA.GetDataList(1, nodes);

            if (nodes.Count > 0)
            {
                var selectedZs = GetZoneFromNode(allBps, nodes);

                DA.SetDataList(0, selectedZs);
                return;
            }
            else
            {
                DA.SetDataList(0, allBps);
            }


            

            //var selectedZones = PickZones(allBps);
           
            //var outBx = new List<Rhino.Geometry.Brep>();


            //scopeParm..PersistentData.AppendRange(outBx2);


            //DA.SetDataList(0, selectedZones);
            //DA.SetDataList(0, a);

            //Rhino.DocObjects.ObjRef[] selbxs;
            //var rc = Rhino.Input.RhinoGet.GetMultipleObjects("Select zone nodes",true, Rhino.DocObjects.ObjectType.Brep, out selbxs);

            //if (rc == Rhino.Commands.Result.Success)
            //{
            //    var outBx = new List<Rhino.Geometry.Brep>();
            //    foreach (var item in selbxs)
            //    {
            //        outBx.Add(item.Brep().DuplicateBrep());
            //    }
            //    doc.Objects.Delete(nodeIds, false);
            //    //foreach (var item in nodeIds)
            //    //{
            //    //    var foundObj = Rhino.RhinoDoc.ActiveDoc.Objects.Find(item);

            //    //}
            //    var selectedZones = new List<GH_Brep>();

            //    foreach (var item in allBps)
            //    {
            //        foreach (var b in outBx)
            //        {
            //            var inz = b.GetBoundingBox(false).Contains(item.Value.GetBoundingBox(false).Center);
            //            if (inz)
            //            {
            //                selectedZones.Add(item);
            //            }
            //        }
            //    }

            //    DA.SetDataList(0, selectedZones);
            //}

            //var go = new Rhino.Input.Custom.GetObject();

        }

        private static List<GH_Brep> GetZoneFromNode(List<GH_Brep> allBps, List<GH_Brep> outBx)
        {
            var selectedZones = new List<GH_Brep>();
            foreach (var item in allBps)
            {
                var sBp = item.DuplicateBrep();
                //((IGH_GeometricGoo)item).LoadGeometry();
                foreach (var b in outBx)
                {
                    var inz = b.Value.GetBoundingBox(false).Contains(sBp.Value.GetBoundingBox(false).Center);
                    if (inz)
                    {
                        selectedZones.Add(item);
                    }
                }
            }

            return selectedZones;
        }

        public override void CreateAttributes()
        {
            var att = new IB_ComponentButtonAttributes(this);
            att.ButtonText = "Pick zones";
            att.MouseDownEvent += (object obj) => this.PickZones();
            this.Attributes = att;
        }

        private void PickZones()
        {
            var doc = Rhino.RhinoDoc.ActiveDoc;
            var att = new Rhino.DocObjects.ObjectAttributes();
            var sz = 0.5;
            var allBps = this.AllInputBreps;

            if (allBps.Count==0)
            {
                return;
            }

            var nodeIds = new List<Guid>();
            foreach (var item in allBps)
            {
                //var c = Rhino.Geometry.AreaMassProperties.Compute(item.Value).Centroid;
                var bdBox = item.Value.GetBoundingBox(false);
                var cc = bdBox.Center;
                Rhino.Geometry.Point3d pt0 = new Rhino.Geometry.Point3d(-sz, -sz, -sz);
                Rhino.Geometry.Point3d pt1 = new Rhino.Geometry.Point3d(sz, sz, sz);

                Rhino.Geometry.BoundingBox box = new Rhino.Geometry.BoundingBox(pt0, pt1);
                var trans = Rhino.Geometry.Transform.Translation(new Rhino.Geometry.Vector3d(cc));
                box.Transform(trans);
                var bbox = new GH_Box(box);
                var id = new Guid();

                if (bbox.BakeGeometry(doc, att, ref id))
                {
                    nodeIds.Add(id);
                }
                //DA.SetData(0, box);

                //var go = new GetObject();

            }
            doc.Objects.UnselectAll();
            doc.Views.Redraw();

            GH.Instances.DocumentEditor.FadeOut();
            //GH.Instances.DocumentEditor.lo
            var a = GH.Getters.GH_BrepGetter.GetBreps();
            GH.Instances.DocumentEditor.FadeIn();

            var outBx2 = new List<GH_Brep>();
            var scopeParm = this.Params.Input[1] as GH.Kernel.GH_PersistentGeometryParam<GH.Kernel.Types.GH_Brep>;
            scopeParm.ClearData();
            scopeParm.PersistentData.Clear();
            //scopeParm.SetPersistentData(a);
            foreach (var item in a)
            {
                ((IGH_GeometricGoo)item).LoadGeometry();
                var newitem = new GH_Brep(item.Value.DuplicateBrep());
                //var new2 = item.DuplicateBrep(); 
                scopeParm.PersistentData.Append(newitem);
                outBx2.Add(newitem);
                //outBx.Add(new3);
            }
            
            //scopeParm.ExpireSolution(false);
            scopeParm.CollectData();
            
            var selectedZones = GetZoneFromNode(allBps, outBx2);
            
            var outParm = this.Params.Output[0] as GH.Kernel.GH_PersistentGeometryParam<GH.Kernel.Types.GH_Brep>;
            outParm.PersistentData.Clear();
            outParm.SetPersistentData(selectedZones);
            //outParm.PersistentData.AppendRange(selectedZones);
            outParm.CollectData();
            outParm.ComputeData();
            //outParm.AddVolatileDataList(new GH.Kernel.Data.GH_Path(), selectedZones);
            doc.Objects.Delete(nodeIds, true);
            //return selectedZones;
        }

    }
}