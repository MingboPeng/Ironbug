using Eto.Forms;
using Ironbug.RhinoOpenStudio.GeometryConverter;
using Rhino.DocObjects;
using Rhino.UI;

namespace Ironbug.RhinoOpenStudio
{
    public class OsmPropertyPanel : ObjectPropertiesPage
    {
        private OsmPropertyPanelUC m_control;
        private EtoPanel0 etoPanel;

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
            if (selectedObj is RHIB_SubSurface subSurface)
            {
                var userdata = subSurface.BrepGeometry.Surfaces[0].UserData.Find(typeof(OsmObjectData)) as OsmObjectData;
                if (!string.IsNullOrWhiteSpace(userdata.Notes))
                {
                    etoPanel.TextBox.Text = userdata.Notes;
                    etoPanel.Label.Text = subSurface.Name;
                }
            }
            else if (selectedObj is RHIB_Space zone)
            {
                var userdata = zone.BrepGeometry.UserData.Find(typeof(OsmObjectData)) as OsmObjectData;
                if (!string.IsNullOrWhiteSpace(userdata.Notes))
                {
                    etoPanel.TextBox.Text = userdata.Notes;
                    etoPanel.Label.Text = zone.Name;
                }
            }




            //base.UpdatePage(e);

        }

        //public override object PageControl => m_control ?? (m_control = new OsmPropertyPanelUC());
        public override object PageControl => etoPanel ?? (etoPanel = new EtoPanel0());
    }

    internal class EtoPanel0 : Eto.Forms.Panel
    {
        public EtoPanel0() : base()
        {
            InitializeComponent();
        }

        public Label Label;
        public TextBox TextBox;

        /// <summary>
        /// Create Panel.Content
        /// </summary>
        private void InitializeComponent()
        {
            this.TextBox = new TextBox();
            this.Label = new Label();
            Content = new TableLayout
            {
                Spacing = new Eto.Drawing.Size(4, 4),
                Rows =
                {
                    this.Label,
                    new TableRow(TextBox) { ScaleHeight = true },
                    new StackLayout
                    {
                    Padding = 0,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    Spacing = 4,
                    Items =
                    {
                      new Label(){Text = "label2"},
                    }
                    }
                }
            };
        }
    }
}