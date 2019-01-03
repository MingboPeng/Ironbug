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
        readonly BoundingBox _bbox;
        private (List<Brep> Roof, List<Brep> Wall, List<Brep> Floor) m_ObjectToBeShown;
        public OsmObjDisplayConduit()
        {
            this.UpdateOsmObjects();
        }

        private void UpdateOsmObjects()
        {
            var s = new Rhino.DocObjects.ObjectEnumeratorSettings();
            s.HiddenObjects = true;
            s.NormalObjects = true;
            s.ObjectTypeFilter = Rhino.DocObjects.ObjectType.Brep;

            var allObjs = Rhino.RhinoDoc.ActiveDoc.Objects.GetObjectList(s);

            var roofToBeShown = new List<Brep>();
            var wallToBeShown = new List<Brep>();
            var floorToBeShown = new List<Brep>();

            var m = new OpenStudio.Model();
            foreach (var obj in allObjs)
            {
                if (obj.Geometry.ObjectType != Rhino.DocObjects.ObjectType.Brep)
                    continue;

                var osmObj = obj as RHIB_Space;
                if (null == osmObj)
                    continue;

                var spaceBrep = osmObj.BrepGeometry;
                var srfBrepfaces = spaceBrep.Faces;
                //var spaceSrfs = spaceBrep.Surfaces;

                foreach (var item in srfBrepfaces)
                {
                    //get idfstring
                    var srfID = item.GetCentorAreaForID();

                    var idfString = osmObj.GetSurfaceIdfString(srfID);

                    //get idfObject 
                    var idfObj = OpenStudio.IdfObject.load(idfString).get();
                    var osmSurf = m.addObject(idfObj).get().to_Surface().get();
                    //get object info
                    var isPartOfEnvelope = osmSurf.outsideBoundaryCondition().ToLower() == "outdoors";
                    var surfaceType = osmSurf.surfaceType();
                    
                    //var isPartOfEnvelope = objData.OsmObjProperties.GetBool("isPartOfEnvelope");
                    //var surfaceType = objData.OsmObjProperties.GetString("surfaceType");

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
                        floorToBeShown.Add(b);
                    }

                }

            }

            this.m_ObjectToBeShown = (roofToBeShown, wallToBeShown, floorToBeShown);
            
        }

       
        protected override void CalculateBoundingBox(CalculateBoundingBoxEventArgs e)
        {
            base.CalculateBoundingBox(e);
            e.BoundingBox.Union(e.Display.Viewport.ConstructionPlane().Origin);
            
        }

        

        protected override void PreDrawObjects(DrawEventArgs e)
        {
            base.PreDrawObjects(e);
            
            var mat = new DisplayMaterial(System.Drawing.Color.FromArgb(112,150,131,74),0.2);
            var matRoof = new DisplayMaterial(System.Drawing.Color.FromArgb(112,112,57,57),0.5);
            var objsToBeShown = this.m_ObjectToBeShown;

            var walls = objsToBeShown.Wall;
            foreach (var item in walls)
            {
                e.Display.DrawBrepShaded(item, mat);
                e.Display.DrawBrepWires(item, System.Drawing.Color.FromArgb(112, 150, 131, 74), 2);
            }
            var roofs = objsToBeShown.Roof;
            foreach (var item in roofs)
            {
                e.Display.DrawBrepShaded(item, matRoof);
                e.Display.DrawBrepWires(item, System.Drawing.Color.FromArgb(112, 112, 57, 57), 2);
            }

         
        }
    }
}
