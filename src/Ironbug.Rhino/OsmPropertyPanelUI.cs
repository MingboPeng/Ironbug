using Eto.Forms;
using Ironbug.RhinoOpenStudio.GeometryConverter;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Ironbug.RhinoOpenStudio
{
    internal class OsmPropertyPanelUI : Eto.Forms.Panel
    {
        //public RhinoObject SelectedObj { get; set; }
        private OsmObjectData OsmObject;

        public OsmPropertyPanelUI() : base()
        {
            InitializeComponent();
        }

        private static OpenStudio.Model exampleOSModel;

        private OpenStudio.Model ExampleOSModel
        {
            get
            {
                if (exampleOSModel == null)
                {
                    exampleOSModel = OpenStudio.OpenStudioModelCore.exampleModel();
                }
                return exampleOSModel;
            }
        }

        private static TableLayout osSpaceLayout;

        private TableLayout OsSpaceLayout
        {
            get
            {
                if (osSpaceLayout == null)
                {
                    osSpaceLayout = this.CreateLayout(this.ExampleOSModel.getSpaces()[0].idfObject());
                }
                return osSpaceLayout;
            }
        }

        private static TableLayout osSubSurfaceLayout;

        private TableLayout OsSubSurfaceLayout
        {
            get
            {
                if (osSubSurfaceLayout == null)
                {
                    osSubSurfaceLayout = this.CreateLayout(this.ExampleOSModel.getSubSurfaces()[0].idfObject());
                }
                return osSubSurfaceLayout;
            }
        }

        private static TableLayout osSurfaceLayout;

        public TableLayout OsSurfaceLayout
        {
            get
            {
                if (osSurfaceLayout == null)
                {
                    osSurfaceLayout = this.CreateLayout(this.ExampleOSModel.getSurfaces()[0].idfObject());
                }
                return osSurfaceLayout;
            }
        }

        private static IDictionary<string, string> osSpaceTypes;

        private IDictionary<string, string> OsSpaceTypes
        {
            get
            {
                if (osSpaceTypes == null)
                {
                    osSpaceTypes = GetSpaceTypes();
                }
                return osSpaceTypes;
            }
        }

        /// <summary>
        /// Create Panel.Content
        /// </summary>
        private void InitializeComponent()
        {
            //initialize table layouts for OS:Space, Surface, SubSurface
            var a = this.OsSpaceLayout;
            var b = this.OsSurfaceLayout;
            var c = this.OsSubSurfaceLayout;
            
        }

        //Populate all idf items
        public bool PopulateIdfData(OsmObjectData osmObjectData)
        {
            var success = false;
            this.OsmObject = osmObjectData;

            var idfObject = OpenStudio.IdfObject.load(osmObjectData.IDFString).get();

            var data = idfObject.GetUserFriendlyFieldInfo().ToList();
            var rowCounts = data.Count * 2 + 1;

            var osType = idfObject.iddObject().type().valueDescription();

            TableLayout layout = null;
            if (osType == "OS:Space")
            {
                layout = this.OsSpaceLayout;
            }
            else if (osType == "OS:SubSurface")
            {
                layout = this.OsSubSurfaceLayout;
            }
            else if (osType == "OS:Surface")
            {
                layout = this.OsSurfaceLayout;
            }

            if (layout == null) return false;

            var count = 0;
            foreach (var item in data)
            {
                var inputControl = layout.Controls.ToList()[(count * 2) + 1];

                if (inputControl is TextBox textBox)
                {
                    var valueToShow = item.DataValue;
                    var match = MatchGUIDString(valueToShow);

                    if (string.IsNullOrWhiteSpace(valueToShow))
                    {
                        //do nothing
                    }
                    else if (match)
                    {
                        var m = IronbugRhinoPlugIn.Instance.OsmModel;
                        var osObj = m.getObject(OpenStudio.OpenStudioUtilitiesCore.toUUID(valueToShow));
                        if (osObj.is_initialized())
                        {
                            valueToShow = osObj.get().nameString();
                        }
                        textBox.Enabled = false;
                    }
                    else
                    {
                    }
                    textBox.Text = valueToShow;
                }
                else if (inputControl is DropDown dropDown)
                {
                    if (item.FieldInfo.ValidData.Any())
                    { // choice type
                        var itemIndex = item.FieldInfo.ValidData.Select(_ => _.ToLower()).ToList().IndexOf(item.DataValue.ToLower());
                        dropDown.SelectedIndex = itemIndex;
                    }
                    else //object-list type
                    {
                        dropDown.SelectedKey = item.DataValue;
                    }
                    
                }
                
                count++;
            }

            this.Content = layout;
            success = true;
            return success;

            bool MatchGUIDString(string s)
            {
                var match = Regex.Match(s, @"\{[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}\}", RegexOptions.IgnoreCase);
                return match.Success;
            }
        }

        private void DropDown_SelectedKeyChanged(object sender, System.EventArgs e)
        {
            var s = sender as DropDown;
            var k = s.SelectedKey;
            var v = s.SelectedValue;

            if (this.OsmObject == null)
                return;

            if (!s.HasFocus)
                return;

            var iddFieldIndex = (int)s.Tag;

            try
            {
                if (!this.OsmObject.UpdateIdfString(iddFieldIndex, k))
                {
                    throw new System.Exception("Failed to update the value");
                }
                else
                {
                    Rhino.RhinoApp.WriteLine("updated to {0} : {1}", k, v);
                }
            }
            catch (System.Exception ee)
            {
                throw new System.Exception(ee.Message);
            }
        }

        private bool _textChanged = false;

        private void InputBox_TextChanged(object sender, System.EventArgs e)
        {
            var s = sender as TextBox;
            if (!s.HasFocus)
                return; //do no fire this event when updated not manually.

            _textChanged = true;
        }

        private void InputBox_LostFocus(object sender, System.EventArgs e)
        {
            if (!_textChanged)
                return;

            _textChanged = false;

            var s = sender as TextBox;

            if (s.HasFocus)
                return;

            if (this.OsmObject == null)
                return;

            var iddFieldIndex = (int)s.Tag;

            try
            {
                if (!this.OsmObject.UpdateIdfString(iddFieldIndex, s.Text))
                {
                    throw new System.Exception("Failed to update the value");
                }
                else
                {
                    Rhino.RhinoApp.WriteLine("{0} has been changed to: {1}", s.Tag, s.Text);
                }
            }
            catch (System.Exception ee)
            {
                throw new System.Exception(ee.Message);
            }
        }

        //private TableLayout CreateSpaceLayout()
        //{
        //    var osSpace = this.ExampleOSModel.getSpaces()[0];
        //    var idfObject = osSpace.idfObject();
        //    var layout = CreateLayout(idfObject);

        //    return layout;
        //}

        //private TableLayout CreateSubSurfaceLayout()
        //{
        //    var osObj = this.ExampleOSModel.getSubSurfaces()[0];
        //    var idfObject = osObj.idfObject();
        //    var layout = CreateLayout(idfObject);

        //    return layout;
        //}

        private TableLayout CreateLayout(OpenStudio.IdfObject idfObject)
        {
            var data = idfObject.GetUserFriendlyFieldInfo().ToList();
            var rowCounts = data.Count * 2 + 1;
            var layout = new TableLayout(1, rowCounts);
            layout.Spacing = new Eto.Drawing.Size(5, 5);
            layout.Padding = new Eto.Drawing.Padding(10);

            var count = 0;
            foreach (var item in data)
            {
                layout.Add(new Label { Text = string.Format("{0} {1}", item.DataName, item.DataUnit) }, 0, count * 2);
                Control ctrl;
                
                if (item.FieldInfo.DataType.ToLower() == "choice" )
                {
                    var ls = new DropDown
                    {
                        Tag = item.FieldInfo.DataFieldIndex
                    };
                    foreach (var d in item.FieldInfo.ValidData)
                    {
                        ls.Items.Add(d);
                    }

                    ls.SelectedKeyChanged += DropDown_SelectedKeyChanged;
                    ctrl = ls;

                }
                else if (item.DataName == "Space Type Name") //TODO: fix this later. No hard coded!!! This is for object-list type field
                {
                    var ls = new DropDown
                    {
                        Tag = item.FieldInfo.DataFieldIndex
                    };
                    foreach (var spaceType in this.OsSpaceTypes)
                    {
                        ls.Items.Add(spaceType.Value, spaceType.Key);
                    }

                    ls.SelectedKeyChanged += DropDown_SelectedKeyChanged;
                    ctrl = ls;
                }
                else
                {
                    var textBox = new TextBox { Tag = item.FieldInfo.DataFieldIndex };
                    textBox.LostFocus += InputBox_LostFocus;
                    textBox.TextChanged += InputBox_TextChanged;

                    ctrl = textBox;
                }
                layout.Add(ctrl, 0, count * 2 + 1);
                count++;
            }
            layout.Add(null, 0, rowCounts - 1);// add an empty row at the end.

            return layout;
        }

        private Dictionary<string, string> GetSpaceTypes()
        {
            var model = IronbugRhinoPlugIn.Instance.OsmModel;
            var dic = new Dictionary<string, string>();

            var types = model.getSpaceTypes();
            foreach (var item in types)
            {
                dic.Add(item.handle().__str__(), item.nameString());
            }

            return dic;
        }
    }
}