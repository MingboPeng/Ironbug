using Rhino.Display;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ironbug.RhinoOpenStudio.GeometryConverter;

namespace Ironbug.RhinoOpenStudio
{
    class OsmObjDisplayConduit : Rhino.Display.DisplayConduit
    {
        private (List<Brep> Roof, List<Brep> Wall, List<Brep> Floor) m_ObjectToBeShown;
        public OsmObjDisplayConduit()
        {
            var s = new Rhino.DocObjects.ObjectEnumeratorSettings();
            s.HiddenObjects = true;
            s.NormalObjects = true;
            s.ObjectTypeFilter = Rhino.DocObjects.ObjectType.Brep;

            var allObjs = Rhino.RhinoDoc.ActiveDoc.Objects.GetObjectList(s);

            var roofToBeShown = new List<Brep>();
            var wallToBeShown = new List<Brep>();
            var floorToBeShown = new List<Brep>();

            foreach (var obj in allObjs)
            {


                if (obj.Geometry.ObjectType != Rhino.DocObjects.ObjectType.Brep)
                    continue;

                var osmObj = obj as RHIB_Space;
                if (null == osmObj)
                    continue;

                var spaceBrep = osmObj.BrepGeometry;
                var srfBrepfaces = spaceBrep.Faces;
                var spaceSrfs = spaceBrep.Surfaces;
                foreach (var item in srfBrepfaces)
                {
                    var srf = item.UnderlyingSurface();
                    var objData = srf.UserData.Find(typeof(OsmObjectData)) as OsmObjectData;
                    var isPartOfEnvelope = objData.OsmObjProperties.GetBool("isPartOfEnvelope");
                    var surfaceType = objData.OsmObjProperties.GetString("surfaceType");

                    if (!isPartOfEnvelope)
                        continue;

                    var b = item.DuplicateFace(false);
                    if (surfaceType == "RoofCeiling")
                    {
                        roofToBeShown.Add(b);
                    }
                    else if (surfaceType == "Wall")
                    {
                        wallToBeShown.Add(b);
                    }
                    else if (surfaceType == "Floor")
                    {
                        //floorToBeShown.Add(b);
                    }

                }
                
            }

            this.m_ObjectToBeShown = (roofToBeShown,wallToBeShown,floorToBeShown);




        }
        protected override void CalculateBoundingBox(CalculateBoundingBoxEventArgs e)
        {
            base.CalculateBoundingBox(e);
            e.BoundingBox.Union(e.Display.Viewport.ConstructionPlane().Origin);
        }

        

        protected override void PreDrawObjects(DrawEventArgs e)
        {
            base.PreDrawObjects(e);
            
            var mat = new DisplayMaterial(System.Drawing.Color.SteelBlue, 0.1);
            var matRoof = new DisplayMaterial(System.Drawing.Color.BurlyWood, 0.5);
            var objsToBeShown = this.m_ObjectToBeShown;

            var walls = objsToBeShown.Wall;
            foreach (var item in walls)
            {
                e.Display.DrawBrepShaded(item, mat);
                e.Display.DrawBrepWires(item, System.Drawing.Color.SteelBlue, 2);
            }
            var roofs = objsToBeShown.Roof;
            foreach (var item in roofs)
            {
                e.Display.DrawBrepShaded(item, matRoof);
                e.Display.DrawBrepWires(item, System.Drawing.Color.BurlyWood, 2);
            }

            //var allGameObjects = e.RhinoDoc.Objects;

            //var s = new Rhino.DocObjects.ObjectEnumeratorSettings();
            //s.HiddenObjects = true;
            //s.NormalObjects = true;
            //s.ObjectTypeFilter = Rhino.DocObjects.ObjectType.Brep;

            //var allObjs = e.RhinoDoc.Objects.GetObjectList(s);

            //foreach (var go in allObjs)
            //{


            //    if (go.Geometry.ObjectType != Rhino.DocObjects.ObjectType.Brep)
            //        continue;

            //    var osmObj = go as RHIB_Space;
            //    if (null == osmObj)
            //        continue;

            //    var spaceBrep = osmObj.BrepGeometry;
            //    var srfBrepfaces = spaceBrep.Faces;
            //    var spaceSrfs = spaceBrep.Surfaces;
            //    foreach (var item in srfBrepfaces)
            //    {
            //        var srf = item.UnderlyingSurface();
            //        var objData = srf.UserData.Find(typeof(OsmObjectData)) as OsmObjectData;
            //        var objDataStr = objData.Notes;
            //        if (objDataStr.Contains("Wall,                                   !- Surface Type"))
            //        {
            //            var srfB = item.ToBrep();
            //            e.Display.DrawBrepShaded(srfB, mat);
            //            e.Display.DrawBrepWires(srfB, System.Drawing.Color.SteelBlue, 2);
            //        }

            //    }
            //}
        }
    }
}
