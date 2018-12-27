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

        public override bool ShouldDisplay(ObjectPropertiesPageEventArgs e)
        {
            if (e.ObjectCount != 1) return false;

            var selectedObj = e.Objects[0];
            var isOSM = selectedObj is RHIB_SubSurface;
            isOSM |= selectedObj is RHIB_Space;
            //return rhObj is RHIB_SubSurface;
            return isOSM;
        }

        public override ObjectType SupportedTypes => ObjectType.Brep;
        public override bool SupportsSubObjects => true;

        public override void UpdatePage(ObjectPropertiesPageEventArgs e)
        {
            if (e.ObjectCount != 1) return;

            var selectedObj = e.Objects[0];

            var idfString = string.Empty;
            if (selectedObj is RHIB_SubSurface subSurface)
            {
                var userdata = subSurface.BrepGeometry.Surfaces[0].UserData.Find(typeof(OsmObjectData)) as OsmObjectData;
                idfString = userdata.Notes;
            }
            else if (selectedObj is RHIB_Space zone)
            {
                var userdata = zone.BrepGeometry.UserData.Find(typeof(OsmObjectData)) as OsmObjectData;
                idfString = userdata.Notes;
            }

            //add to panel
            if (!string.IsNullOrWhiteSpace(idfString))
            {
                try
                {
                    var idfobj = OpenStudio.IdfObject.load(idfString).get();
                    this.panelUI.PopulateIdfData(idfobj);
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