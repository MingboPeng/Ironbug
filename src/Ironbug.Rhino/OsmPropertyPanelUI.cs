using Eto.Forms;
using System.Collections.Generic;
using System.Linq;
using Ironbug.RhinoOpenStudio.GeometryConverter;

namespace Ironbug.RhinoOpenStudio
{
    internal class OsmPropertyPanelUI : Eto.Forms.Panel
    {
        public OsmPropertyPanelUI() : base()
        {
            InitializeComponent();
        }
        private static TableLayout osSpaceLayout;

        private TableLayout OsSpaceLayout
        {
            get {
                if (osSpaceLayout == null)
                {
                    osSpaceLayout = CreateSpaceLayout();
                }
                return osSpaceLayout;
            }
        }
        
        private TableLayout subsurfaceLayout;
        /// <summary>
        /// Create Panel.Content
        /// </summary>
        private void InitializeComponent()
        {
            //TODO: initialize table layouts for OS:Space, Surface, SubSurface 
        }

        //Populate all idf items
        public bool PopulateIdfData(IEnumerable<(string DataName, string DataValue, string DataUnit)> FormatedString)
        {
            var success = false;

            var data = FormatedString.ToList();
            var rowCounts = data.Count * 2 + 1;
            var layout = this.OsSpaceLayout;

            var count = 0;
            foreach (var item in data)
            {
                var inputBox = layout.Controls.ToList()[(count * 2) + 1] as TextBox;
                if (inputBox != null)
                {
                    inputBox.Text = item.DataValue;
                }
                //layout.Add(new TextBox { Text = item.DataValue }, 0, count * 2 + 1);
                count++;
            }

            this.Content = layout;

            success = true;

            return success;
        }

        private TableLayout CreateSpaceLayout()
        {
            var m = new OpenStudio.Model();
            var osSpace = new OpenStudio.Space(m);
            osSpace.setThermalZone(new OpenStudio.ThermalZone(m));
            var idfObject = osSpace.idfObject();
            var idfStr = idfObject.__str__();
            var data = idfObject.GetUserFriendlyFieldInfo().ToList();
            var rowCounts = data.Count * 2 + 1;
            var layout = new TableLayout(1, rowCounts);
            layout.Spacing = new Eto.Drawing.Size(5, 5);
            layout.Padding = new Eto.Drawing.Padding(10);

            var count = 0;
            foreach (var item in data)
            {
                layout.Add(new Label { Text = string.Format("{0} {1}", item.DataName, item.DataUnit) }, 0, count * 2);
                layout.Add(new TextBox {  }, 0, count * 2 + 1);
                count++;
            }
            layout.Add(null, 0, rowCounts - 1);// add an empty row at the end.

            return layout;
        }

        private void CreateSubSurfaceLayout()
        {

        }

        private void GetSpaceTypes()
        {

        }
    }
}