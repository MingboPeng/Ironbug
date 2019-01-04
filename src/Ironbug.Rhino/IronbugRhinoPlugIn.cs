using Ironbug.Core.OpenStudio;
using Ironbug.RhinoOpenStudio.GeometryConverter;
using Rhino;
using Rhino.DocObjects;
using Rhino.FileIO;
using Rhino.PlugIns;
using System.Collections.Generic;
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

        //Access this by IronbugRhinoPlugIn.Instance.OsmFileString;
        private OsmDocumentData OsmFileString { get; set; } = new OsmDocumentData();

        //public string OsmFilePath { get; private set; } = string.Empty;

        //Access this by IronbugRhinoPlugIn.Instance.OsmModel;
        public OpenStudio.Model OsmModel { get; private set; }

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

            if (subsurfaceLoadedCount > 0 || spaceLoadedCount > 0)
            {
                RhinoApp.WriteLine("{0} OS:Space; {1} OS:Subsurface loaded", spaceLoadedCount, subsurfaceLoadedCount);
                RhinoDoc.ReplaceRhinoObject += RhinoDoc_ReplaceRhinoObject;
                this.ReadTempOsmModelToThisDoc();
            }
            //Rhino.UI.Dialogs.ShowMessage("file open event end", "test");
        }

        private void RhinoDoc_ReplaceRhinoObject(object sender, RhinoReplaceObjectEventArgs e)
        {
            if (e.OldRhinoObject is RHIB_SubSurface && e.NewRhinoObject is RHIB_SubSurface)
            {
                //TODO: Check if new rhino obj has the same osm handle id as old rhino obj.
                //for now, they are the same.

                var newobj = e.NewRhinoObject as RHIB_SubSurface;

                if (newobj.Update())
                    RhinoApp.WriteLine("one surface updated #####################################");
            }
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
                //this.OsmFilePath = filename;
                ReadOsmStringToDoc(filename);
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
                        doc.Objects.AddRhinoObject(glz);
                    }

                    doc.Objects.AddRhinoObject(space);
                    space.Attributes.LayerIndex = layerIndex;
                    space.CommitChanges();
                    spaceAddedCount++;
                }

                Rhino.UI.Dialogs.ShowMessage(spaceAddedCount + " OpenStudio spaces loaded", "Open OpenStudio model");
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

        private void ReadOsmStringToDoc(string osmFilePath)
        {
            OsmFileString.Add(File.ReadAllText(osmFilePath));
        }

        private void ReadTempOsmModelToThisDoc()
        {
            if (OsmFileString.Count == 0) return;

            var tempPath = Path.GetTempPath() + @"\Ironbug\TempOpenStudio";
            Directory.CreateDirectory(tempPath);

            var tempFile = Path.Combine(tempPath, "temp.osm");
            File.WriteAllText(tempFile, OsmFileString.Item(0));
            if (File.Exists(tempFile))
            {
                var p = OpenStudio.OpenStudioUtilitiesCore.toPath(tempFile);
                OsmModel = OPS.Model.load(p).get();
            }
        }

        /// <summary>
        /// OnCloseDocument event handler.
        /// </summary>
        private void OnCloseDocument(object sender, DocumentEventArgs e)
        {
            this.OsmFileString.Clear();
            if (this.OsmModel != null)
            {
                this.OsmModel = null;
            }
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
            this.OsmFileString.WriteDocument(archive);
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

        protected override void ObjectPropertiesPages(List<Rhino.UI.ObjectPropertiesPage> pages)
        {
            if (null == pages)
                return;
            OsmPropertyPanel objectPropertiesPage = new OsmPropertyPanel();
            pages.Add(objectPropertiesPage);
        }
    }
}