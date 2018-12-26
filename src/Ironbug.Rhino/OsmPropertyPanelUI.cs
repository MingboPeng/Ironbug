using Eto.Forms;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.RhinoOpenStudio
{
    internal class OsmPropertyPanelUI : Eto.Forms.Panel
    {
        public OsmPropertyPanelUI() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create Panel.Content
        /// </summary>
        private void InitializeComponent()
        {
        }

        //Populate all idf items
        public bool PopulateIdfData(IEnumerable<(string DataName, string DataValue, string DataUnit)> FormatedString)
        {
            var success = false;
            var data = FormatedString.ToList();

            var layout = new TableLayout(1, data.Count * 2);
            layout.Spacing = new Eto.Drawing.Size(5, 5);
            layout.Padding = new Eto.Drawing.Padding(10);

            var count = 0;
            foreach (var item in data)
            {
                layout.Add(new Label { Text = string.Format("{0} {1}", item.DataName, item.DataUnit) }, 0, count * 2);
                layout.Add(new TextBox { Text = item.DataValue }, 0, count * 2 + 1);
                count++;
            }

            this.Content = layout;

            success = true;

            return success;
        }
    }
}