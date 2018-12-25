using Ironbug.Core.OpenStudio;
using Ironbug.RhinoOpenStudio.GeometryConverter;
using Rhino;
using Rhino.DocObjects;
using Rhino.FileIO;
using Rhino.PlugIns;
using System.IO;
using OPS = OpenStudio;

namespace Ironbug.RhinoOpenStudio
{
    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
    public class IronbugRhinoPlugIn : Rhino.PlugIns.FileImportPlugIn

    {
        private const int MAJOR = 1;
        private const int MINOR = 0;
        public OsmDocumentData OsmFileString { get; private set; } = new OsmDocumentData();
        public string OsmFilePath { get; private set; } = string.Empty;

        public object OsmModel { get; private set; }

        public IronbugRhinoPlugIn()
        {
            try
            {
                var osmVersion = "2.7.0.0";
                if (!OpenStudioHelper.LoadAssemblies(osmVersion))
                {
                    Rhino.UI.Dialogs.ShowMessage("Failed to load OpenStudio.dll!", "test");
                }
                else
                {
                    RhinoApp.WriteLine("OpenStudio library {0} loaded", osmVersion);

                }


            }
            catch (FileNotFoundException loadError)
            {
                Rhino.UI.Dialogs.ShowMessage(loadError.Message, "test");
            }
            Instance = this;
            
            RhinoDoc.EndOpenDocument += OnEndOpenDocument;
        }

        protected override LoadReturnCode OnLoad(ref string errorMessage)
        {
            RhinoDoc.CloseDocument += OnCloseDocument;
            return base.OnLoad(ref errorMessage);
        }

        public override PlugInLoadTime LoadTime => PlugInLoadTime.AtStartup;

        private void OnEndOpenDocument(object sender, DocumentOpenEventArgs e)
        {
            

            var objs = e.Document.Objects;
            var subsurfaceLoadedCount = 0;
            var spaceLoadedCount = 0;
            foreach (var item in objs)
            {
                if (item is BrepObject brep)
                {
                    //subsurface
                    var isSrf = brep.BrepGeometry.IsSurface;
                    if (isSrf)
                    {
                        var srfUserdata = brep.BrepGeometry.Surfaces[0].UserData.Find(typeof(OsmObjectData)) as OsmObjectData;
                        if (null == srfUserdata) continue;
                        var subSurface = new RHIB_SubSurface(brep.BrepGeometry);
                        objs.Replace(new ObjRef(brep), subSurface);
                        subsurfaceLoadedCount++;
                    }

                    //space
                    var userdata = brep.BrepGeometry.UserData.Find(typeof(OsmObjectData)) as OsmObjectData;
                    if (null == userdata) continue;

                    var isSolid = brep.BrepGeometry.IsSolid;
                    if (isSolid)
                    {
                        var space = new RHIB_Space(brep.BrepGeometry);
                        objs.Replace(new ObjRef(brep), space);
                        spaceLoadedCount++;

                    }
                }
            }

            if (subsurfaceLoadedCount>0 || spaceLoadedCount>0)
            {
                RhinoApp.WriteLine("{0} OS:Space; {1} OS:Subsurface loaded", spaceLoadedCount, subsurfaceLoadedCount);
                this.OsmModel = new OPS.Model();
            }
            //Rhino.UI.Dialogs.ShowMessage("file open event end", "test");
        }

        ///<summary>Gets the only instance of the IronbugRhinoPlugIn plug-in.</summary>
        public static IronbugRhinoPlugIn Instance
        {
            get; private set;
        }

        ///<summary>Defines file extensions that this import plug-in is designed to read.</summary>
        /// <param name="options">Options that specify how to read files.</param>
        /// <returns>A list of file types that can be imported.</returns>
        protected override Rhino.PlugIns.FileTypeList AddFileTypes(Rhino.FileIO.FileReadOptions options)
        {
            var result = new Rhino.PlugIns.FileTypeList();
            result.AddFileType("OpenStudio Model (*.osm)", "osm");

            return result;
        }

        /// <summary>
        /// Is called when a user requests to import a ."osm file.
        /// It is actually up to this method to read the file itself.
        /// </summary>
        /// <param name="filename">The complete path to the new file.</param>
        /// <param name="index">The index of the file type as it had been specified by the AddFileTypes method.</param>
        /// <param name="doc">The document to be written.</param>
        /// <param name="options">Options that specify how to write file.</param>
        /// <returns>A value that defines success or a specific failure.</returns>
        protected override bool ReadFile(string filename, int index, RhinoDoc doc, Rhino.FileIO.FileReadOptions options)
        {
            bool read_success = false;
            var layerName = "OS:Space";
            var layerIndex = doc.Layers.Add(layerName, System.Drawing.Color.Black);
            var glzLayerIdx = doc.Layers.Add("OS:Glazing", System.Drawing.Color.Blue);

            //Check unit system : meter only
            doc.AdjustModelUnitSystem(UnitSystem.Meters, false);

            if (filename.Contains(".osm"))
            {
                var p = OPS.OpenStudioUtilitiesCore.toPath(filename);
                var tempModel = OPS.Model.load(p);
                if (!tempModel.is_initialized())
                {
                    return false;
                }
                this.OsmFilePath = filename;
                ReadOsmToDoc(filename);
                var model = tempModel.get();
                this.OsmModel = model;
                var sps = model.getSpaces();

                var spaceAddedCount = 0;

                foreach (OPS.Space sp in sps)
                {
                    var (space, glzs) = RHIB_Space.FromOpsSpace(sp);

                    //add glz surfaces to rhino doc.
                    foreach (var glz in glzs)
                    {
                        //doc.Objects.AddBrep(glz);
                        doc.Objects.AddRhinoObject(glz);
                    }

                    if (space.BrepGeometry.IsSolid)
                    {
                        doc.Objects.AddRhinoObject(space);
                        space.Attributes.LayerIndex = layerIndex;
                        space.CommitChanges();
                        spaceAddedCount++;
                    }
                }

                Rhino.UI.Dialogs.ShowMessage(spaceAddedCount + " OpenStudio spaces loaded", "Open OpenStudio model");
                //doc.Views.Redraw();
                read_success = true;
            }
            else
            {
                Rhino.UI.Dialogs.ShowMessage("OSM loaded failed", "test");
                read_success = false;
            }
            return read_success;
        }

        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and maintain plug-in wide options in a document.

        private void ReadOsmToDoc(string osmFilePath)
        {
            OsmFileString.Add(File.ReadAllText(osmFilePath));
        }

        /// <summary>
        /// OnCloseDocument event handler.
        /// </summary>
        private void OnCloseDocument(object sender, DocumentEventArgs e)
        {
            // When the document is closed, clear our
            // document user data containers.
            OsmFileString.Clear();
        }

        protected override bool ShouldCallWriteDocument(FileWriteOptions options)
        {
            return this.OsmFileString.Count > 0;
        }

        /// <summary>
        /// Called when Rhino is saving a .3dm file to allow the plug-in to save document user data.
        /// </summary>
        protected override void WriteDocument(RhinoDoc doc, BinaryArchiveWriter archive, FileWriteOptions options)
        {
            OsmFileString.WriteDocument(archive);
        }

        /// <summary>
        /// Called whenever a Rhino document is being loaded and plug-in user data was
        /// encountered written by a plug-in with this plug-in's GUID.
        /// </summary>
        protected override void ReadDocument(RhinoDoc doc, BinaryArchiveReader archive, FileReadOptions options)
        {
            var string_table = new OsmDocumentData();
            string_table.ReadDocument(archive);

            if (!options.ImportMode && !options.ImportReferenceMode)
            {
                if (string_table.Count > 0)
                    OsmFileString.AddRange(string_table.ToArray());
            }
        }
    }
}