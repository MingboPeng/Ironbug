using Eto.Forms;
using System.Collections.Generic;
using System.Linq;
using Ironbug.RhinoOpenStudio.GeometryConverter;
using System.Text.RegularExpressions;

namespace Ironbug.RhinoOpenStudio
{
    internal class OsmPropertyPanelUI : Eto.Forms.Panel
    {
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
            get {
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

        private static IEnumerable<OpenStudio.SpaceType> osSpaceTypes;

        private IEnumerable<OpenStudio.SpaceType> OsSpaceTypes
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
            //TODO: initialize table layouts for OS:Space, Surface, SubSurface 
        }

        //Populate all idf items
        public bool PopulateIdfData(OpenStudio.IdfObject idfObject)
        {
            var success = false;

            var data = idfObject.GetUserFriendlyFieldInfo().ToList();
            var rowCounts = data.Count * 2 + 1;

            var osType = idfObject.iddObject().type().valueDescription();

            TableLayout layout = null;
            if (osType == "OS:Space" )
            {
                layout = this.OsSpaceLayout;
            }
            else if (osType == "OS:SubSurface")
            {
                layout = this.OsSubSurfaceLayout;
            }

            if (layout == null) return false;

            var count = 0;
            foreach (var item in data)
            {
                var inputBox = layout.Controls.ToList()[(count * 2) + 1] as TextBox;
                if (inputBox != null)
                {
                    var valueToShow = item.DataValue;
                    var match = MatchGUIDString(valueToShow);

                    if (string.IsNullOrWhiteSpace(valueToShow))
                    {
                        //do nothing
                    }
                    //else if (item.DataName == "Space Type Name")
                    //{
                    //    var spaceType = this.OsSpaceTypes.First(_ => _.handle().__str__() == valueToShow);
                    //    valueToShow = spaceType.nameString();
                    //}
                    else if (match)
                    {
                        var m = IronbugRhinoPlugIn.Instance.OsmModel;
                        var osObj = m.getObject(OpenStudio.OpenStudioUtilitiesCore.toUUID(valueToShow));
                        if (osObj.is_initialized())
                        {
                            valueToShow = osObj.get().nameString();
                        }
                    }
                    inputBox.Text = valueToShow;
                }
                //layout.Add(new TextBox { Text = item.DataValue }, 0, count * 2 + 1);
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
                layout.Add(new TextBox { }, 0, count * 2 + 1);
                count++;
            }
            layout.Add(null, 0, rowCounts - 1);// add an empty row at the end.

            return layout;
        }

        private IEnumerable<OpenStudio.SpaceType> GetSpaceTypes()
        {
            var model = IronbugRhinoPlugIn.Instance.OsmModel;
            var typedic = new Dictionary<string, string>();

            var types = model.getSpaceTypes().ToArray();

            return types;

        }
    }
}