﻿using Ironbug.RhinoOpenStudio.GeometryConverter;
using Rhino.DocObjects;
using Rhino.UI;

namespace Ironbug.RhinoOpenStudio
{
    public class OsmPropertyPanel : ObjectPropertiesPage
    {
        //private OsmPropertyPanelUC m_control;
        private OsmPropertyPanelUI panelUI;

        public override string EnglishPageTitle => "OpenStudio Attributes";

        public override ObjectType SupportedTypes => ObjectType.Brep;
        public override bool SupportsSubObjects => true;

        public override bool ShouldDisplay(ObjectPropertiesPageEventArgs e)
        {
            if (e.ObjectCount != 1) return false;
            if (e.Objects.Length != 1) return false; //there is a bug in Rhino, which ObjectCount ==1, but Object is empty.

            //var objs = Rhino.RhinoDoc.ActiveDoc.Objects.GetSelectedObjects(false, false);

            //var go = new Rhino.Input.Custom.GetObject();
            //go.SetCommandPrompt("Select a space or a space's surface");
            //go.GeometryFilter = ObjectType.Brep;
            //go.SubObjectSelect = true;
            //go.Get();

            //var isOSM = false;
            //if (go.CommandResult() == Rhino.Commands.Result.Success)
            //{
            //    //var isOSM = selectedObj is RHIB_SubSurface;
            //    //isOSM |= selectedObj is RHIB_Space;
            //    return true;
            //}
            //return false;

            //var selObj = go.Object(0);
            //var possibleSrf = selObj.Face();
            //var index = selObj.GeometryComponentIndex;
            //var brepobj = selObj.Object() as BrepObject;

            var isOSM = false;
            
            RhinoObject selectedObj = e.Objects[0];

            isOSM = selectedObj is RHIB_SubSurface;
            isOSM |= selectedObj is RHIB_Space;
            
            //return rhObj is RHIB_SubSurface;
            return isOSM;
        }

        

        public override void UpdatePage(ObjectPropertiesPageEventArgs e)
        {
            if (e.ObjectCount != 1) return;
            if (e.Objects.Length != 1) return; //there is a bug in Rhino, which ObjectCount ==1, but Object is empty.

            var selectedObj = e.Objects[0];

            var isSelectedBrepFace = null != selectedObj.GetSelectedSubObjects();
            
            OsmObjectData userdata = null;
            if (isSelectedBrepFace && selectedObj is RHIB_Space)
            {
                var faceIndex = selectedObj.GetSelectedSubObjects()[0].Index;
                var surf = ((RHIB_Space)selectedObj).BrepGeometry.Surfaces[faceIndex];
                userdata = surf.UserData.Find(typeof(OsmObjectData)) as OsmObjectData;
            }
            else if (selectedObj is RHIB_SubSurface subSurface)
            {
                userdata = subSurface.BrepGeometry.Surfaces[0].UserData.Find(typeof(OsmObjectData)) as OsmObjectData;
            }
            else if (selectedObj is RHIB_Space zone)
            {
                userdata = zone.BrepGeometry.UserData.Find(typeof(OsmObjectData)) as OsmObjectData;
            }

            //add to panel
            if (null != userdata)
            {
                try
                {
                    //TODO: I don't think this is the best way to do...fix it later!!
                    this.panelUI.SelectedObj = selectedObj;
                    this.panelUI.PopulateIdfData(userdata);
                }
                catch (System.Exception ex)
                {
                    //Rhino.RhinoApp.WriteLine(ex.Message);
                    Rhino.UI.Dialogs.ShowMessage(ex.Message, "ERROR");
                    //throw ex;
                }
            }
        }

        //public override object PageControl => m_control ?? (m_control = new OsmPropertyPanelUC());
        public override object PageControl => panelUI ?? (panelUI = new OsmPropertyPanelUI());
    }
}