using Ironbug.RhinoOpenStudio.GeometryConverter;
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

            var isOSM = false;
            RhinoObject selectedObj = e.Objects[0];
            isOSM = selectedObj is IRHIB_GeometryBase;
            return isOSM;
        }

        public override void UpdatePage(ObjectPropertiesPageEventArgs e)
        {
           
            var selectedObj = e.Objects[0];

            var isSelectedBrepFace = null != selectedObj.GetSelectedSubObjects();
            
            if (selectedObj is IRHIB_GeometryBase rhib)
            {
                var spaceSurfaceID = string.Empty;

                if (isSelectedBrepFace && selectedObj is RHIB_Space sp)
                {
                    //Surface of space (which is a face of Brep)
                    var faceIndex = selectedObj.GetSelectedSubObjects()[0].Index;
                    spaceSurfaceID = ((RHIB_Space)selectedObj).BrepGeometry.Faces[faceIndex].GetCentorAreaForID();
                }

                try
                {
                    //this.OnActivate(false);
                    //this.OnSizeParent(100, 100);
                    this.panelUI.PopulateIdfData(rhib, spaceSurfaceID);
                    //this.OnActivate(true);
                }
                catch (System.Exception ex)
                {
                    Rhino.UI.Dialogs.ShowMessage(ex.Message, "ERROR");
                }
            }
        }

        public override object PageControl => panelUI ?? (panelUI = new OsmPropertyPanelUI());
    }
}