using Eto.Forms;
using Ironbug.RhinoOpenStudio.GeometryConverter;
using Rhino.DocObjects;
using Rhino.UI;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.RhinoOpenStudio
{
    public class OsmPropertyPanel : ObjectPropertiesPage
    {
        //private OsmPropertyPanelUC m_control;
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
                    var data = idfobj.GetUserFriendlyFieldInfo();
                    this.etoPanel.PopulateIdfData(data);
                }
                catch (System.Exception ex)
                {
                    //Rhino.RhinoApp.WriteLine(ex.Message);
                    Rhino.UI.Dialogs.ShowMessage(ex.Message, "ERROR");
                    //throw ex;
                }
                
                
                //etoPanel.TextBox.Text = userdata.Notes;
                //etoPanel.Label.Text = zone.Name;
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
            this.Label = new Label() { Text = "label1"};


            //var panelLayout = new TableLayout()
            //{
            //    Spacing = new Eto.Drawing.Size(4, 4),
            //    Rows =
            //    {
            //        this.Label,
            //        new TableRow(TextBox) { ScaleHeight = false },
            //        new StackLayout
            //        {
            //        Padding = 0,
            //        HorizontalContentAlignment = HorizontalAlignment.Stretch,
            //        Spacing = 4,
            //        Items ={
            //          new Label(){Text = "label2"},
            //        }
            //        }
            //    }
            //};
            var panelLayout = new TableLayout();
            panelLayout.Spacing = new Eto.Drawing.Size(4, 4);
            panelLayout.Rows.Add(new Label() { Text = "label2" });
            //panelLayout.Rows.Add(new TableRow(TextBox) { ScaleHeight = true });
            //panelLayout.Rows.Add(new StackLayout
            //{
            //    Padding = 0,
            //    HorizontalContentAlignment = HorizontalAlignment.Stretch,
            //    Spacing = 4,
            //    Items =
            //        {
            //          new Label(){Text = "label2"},
            //        }
            //});


            //var layout = new TableLayout(4, 2);
            //layout.Spacing = new Eto.Drawing.Size(5, 5);
            //layout.Padding = new Eto.Drawing.Padding(10);

            //layout.SetColumnScale(1);
            //layout.SetColumnScale(3);
            //layout.SetRowScale(0);
            //layout.SetRowScale(1);

            //layout.Add(new Label { Text = "Default" }, 0, 0);

            //layout.Add(new Label { Text = "No Border" }, 2, 0);

            //layout.Add(new Label { Text = "Bezeled" }, 0, 1);

            //layout.Add(new Label { Text = "Line" }, 2, 1);

            this.Content = panelLayout;
            
        }


        //Populate all idf items
        public bool PopulateIdfData(IEnumerable<(string DataName, string DataValue, string DataUnit)> FormatedString)
        {
            var success = false;
            var data = FormatedString.ToList();


            //var panelLayout = new TableLayout();
            //panelLayout.Spacing = new Eto.Drawing.Size(4, 4);
            //foreach (var item in data)
            //{
            //    var r = new TableRow(new Label { Text = "Second Row" }, new ListBox());
            //    var row = new TableRow(new Label() { Text = item.DataName }, new TextBox() { Text = item.DataValue });

            //    panelLayout.Rows.Add(row);
            //}

            //this.Content = panelLayout;

            //var layout = this.Content as TableLayout;
            //layout.Rows.Clear();

            //layout.Visible = false;
            //layout.SuspendLayout();
            ////layout = new TableLayout()
            ////{
            ////    Spacing = new Eto.Drawing.Size(4, 4),
            ////    Rows =
            ////    {
            ////        new Label(){Text = "label2"}
            ////    }
            ////};
            //layout.Rows.Add(new Label() { Text = "label2rr" });


            ////layout.Update();
            //layout.ResumeLayout();
            //layout.Visible = true;
            ////this.Content = layout;
            ///
            //this.Visible = false;
            //layout.Visible = true;

            //this.SuspendLayout();
            //layout.SuspendLayout();
            //layout.RemoveAll();
            //layout.Spacing = new Eto.Drawing.Size(4, 4);

            //layout.Rows.Add(new Label() { Text = "333label2" });


            //// 
            //// OsmPropertyPanelUC
            //// 
            ////this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            ////this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ////this.Name = "OsmPropertyPanelUC";
            ////this.Size = new System.Drawing.Size(422, 402);
            //layout.ResumeLayout();
            //this.ResumeLayout();

            //layout.Visible = true;
            //this.Visible = true;
            var layout = new TableLayout(1,data.Count*2);
            layout.Spacing = new Eto.Drawing.Size(5, 5);
            layout.Padding = new Eto.Drawing.Padding(10);

            //layout.SetColumnScale(1);
            //layout.SetColumnScale(3);
            //layout.SetRowScale(0);
            //layout.SetRowScale(1);
            //layout.Rows.Add(new Label() { Text = "Defaggggggggggggult" });
            var count = 0;
            foreach (var item in data)
            {
                layout.Add(new Label { Text = string.Format("{0} {1}", item.DataName,item.DataUnit)  }, 0,count*2);
                layout.Add(new TextBox { Text = item.DataValue }, 0, count*2+1);
                count++;
            }

            //layout.Add(new Label { Text = "No Border" }, 2, 0);

            //layout.Add(new Label { Text = "Bezeled" }, 0, 1);

            //layout.Add(new Label { Text = "Line" }, 2, 1);

            this.Content = layout;

            success = true;


            return success;
        }
    }
}