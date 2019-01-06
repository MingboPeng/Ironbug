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
        //private OpenStudio.IdfObject IdfObject;
        private IRHIB_GeometryBase _selectedObject;
        private string _spaceSurfaceCenterAreaID = string.Empty;
            
        public OsmPropertyPanelUI() : base()
        {
            //InitializeComponent();
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
                return osSpaceLayout;
            }
            set
            {
                if (osSpaceLayout == null)
                {
                    osSpaceLayout = value;
                }
            }
        }

        private static TableLayout osSubSurfaceLayout;

        private TableLayout OsSubSurfaceLayout
        {
            get
            {
                return osSubSurfaceLayout;
            }
            set
            {
                if (osSubSurfaceLayout == null)
                {
                    osSubSurfaceLayout = value;
                }
            }
        }

        private static TableLayout osSurfaceLayout;

        public TableLayout OsSurfaceLayout
        {
            get
            {
                return osSurfaceLayout;
            }
            set
            {
                if (osSurfaceLayout == null)
                {
                    osSurfaceLayout = value;
                }
            }
        }

        private static TableLayout osShadingSurfaceLayout;

        public TableLayout OsShadingSurfaceLayout
        {
            get
            {
                return osShadingSurfaceLayout;
            }
            set
            {
                if (osShadingSurfaceLayout == null)
                {
                    osShadingSurfaceLayout = value;
                }
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

        ///// <summary>
        ///// Create Panel.Content
        ///// </summary>
        //private void InitializeComponent()
        //{
        //    //initialize table layouts for OS:Space, Surface, SubSurface
            
        //}


        //Populate all idf items
        public bool PopulateIdfData(IRHIB_GeometryBase rhib, string SpaceSurfaceCenterAreaID = "")
        {
            //assign to property
            this._selectedObject = rhib;
            this._spaceSurfaceCenterAreaID = SpaceSurfaceCenterAreaID;

            var success = false;

            //get idfString
            var idfString = string.Empty;
            if (string.IsNullOrEmpty(SpaceSurfaceCenterAreaID))
            {
                idfString = rhib.GetIdfString();
            }
            else
            {
                idfString = ((RHIB_Space)rhib).GetSurfaceIdfString(SpaceSurfaceCenterAreaID);
            }
            
            //get Ops idfObject
            var idfObject = OpenStudio.IdfObject.load(idfString).get();

            //get field list
            var data = idfObject.GetUserFriendlyFieldInfo().ToList();
            var rowCounts = data.Count * 2 + 1;

            
            var layout = this.GetLayoutByOsType(idfObject);
            if (layout == null)
            {
                throw new System.ArgumentException("Unknown geometry type!");
            }

            var count = 0;
            foreach (var item in data)
            {
                var inputControl = layout.Controls.ToList()[(count * 2) + 1];

                if (inputControl is TextBox textBox)
                {
                    var valueToShow = item.DataValue;
                    var match = MatchGUIDString(valueToShow);

                    if (match)
                    {
                        var m = IronbugRhinoPlugIn.Instance.OsmModel;
                        var osObj = m.getObject(OpenStudio.OpenStudioUtilitiesCore.toUUID(valueToShow));
                        if (osObj.is_initialized())
                        {
                            valueToShow = osObj.get().nameString();
                        }
                        textBox.Enabled = false;
                    }

                    textBox.Text = valueToShow;
                }
                else if (inputControl is DropDown dropDown)
                {
                    if (item.FieldInfo.ValidData.Any())
                    { 
                        // choice type
                        var itemIndex = item.FieldInfo.ValidData.Select(_ => _.ToLower()).ToList().IndexOf(item.DataValue.ToLower());
                        dropDown.SelectedIndex = itemIndex;
                    }
                    else
                    {
                        //object-list type
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

        private TableLayout GetLayoutByOsType(OpenStudio.IdfObject idfObject)
        {
            TableLayout layout = null;
            var osType = idfObject.iddObject().type().valueDescription();
            if (osType == "OS:Space")
            {
                if (OsSpaceLayout == null)
                    OsSpaceLayout = CreateLayout(idfObject);

                layout = this.OsSpaceLayout;
            }
            else if (osType == "OS:SubSurface")
            {
                if (OsSubSurfaceLayout == null)
                    OsSubSurfaceLayout = CreateLayout(idfObject);

                layout = this.OsSubSurfaceLayout;
            }
            else if (osType == "OS:Surface")
            {
                if (OsSurfaceLayout == null)
                    OsSurfaceLayout = CreateLayout(idfObject);

                layout = this.OsSurfaceLayout;
            }
            else if (osType == "OS:ShadingSurface")
            {
                if (OsShadingSurfaceLayout == null)
                    OsShadingSurfaceLayout = CreateLayout(idfObject);

                layout = this.OsShadingSurfaceLayout;
            }

            return layout;
        }

        private void DropDown_SelectedKeyChanged(object sender, System.EventArgs e)
        {
            var s = sender as DropDown;
            var k = s.SelectedKey;
            var v = s.SelectedValue;

            if (this._selectedObject == null)
                return;

            if (!s.HasFocus)
                return;

            var iddFieldIndex = (int)s.Tag;

            var success = this.UpdateObjData(iddFieldIndex, k);
            
            if (success)
            {
                Rhino.RhinoApp.WriteLine("Updated to {0}", v);
            }
            else
            {
                throw new System.Exception("Failed to update the value");
            }

        }

        private bool UpdateObjData(int iddFieldIndex, string Value)
        {
            try
            {
                var success = false;
                if (string.IsNullOrEmpty(this._spaceSurfaceCenterAreaID))
                {
                    //this is a space or subspace (which is a window)
                    success = this._selectedObject.UpdateIdfData(iddFieldIndex, Value);
                }
                else
                {
                    //this is surface of the space 
                    success = this._selectedObject.UpdateIdfData(iddFieldIndex, Value, this._spaceSurfaceCenterAreaID);
                }

                return success;
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

            if (this._selectedObject == null)
                return;

            _textChanged = false;

            var s = sender as TextBox;
            if (s.HasFocus)
                return;
            
            var iddFieldIndex = (int)s.Tag;

            var success = this.UpdateObjData(iddFieldIndex, s.Text);

            if (success)
            {
                Rhino.RhinoApp.WriteLine("Updated to {0}", s.Text);
            }
            else
            {
                throw new System.Exception("Failed to update the value");
            }
        }
        

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