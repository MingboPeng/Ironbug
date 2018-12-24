using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using System;
using System.Linq;
using System.Collections.Generic;
using OPS = OpenStudio;
using Ironbug.RhinoOpenStudio.GeometryConverter;
using System.Reflection;
using System.IO;
using Ironbug.Core.OpenStudio;
using Rhino.FileIO;
using Rhino.PlugIns;

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
        public IronbugRhinoPlugIn()
        {
            try
            {
                if (!OpenStudioHelper.LoadAssemblies())
                {
                    Rhino.UI.Dialogs.ShowMessage("Failed to load OpenStudio.dll!", "test");
                }
                
            }
            catch (FileNotFoundException e)
            {

                Rhino.UI.Dialogs.ShowMessage(e.Message, "test");
            }
            
            Instance = this;
            RhinoDoc.EndOpenDocument += OnEndOpenDocument;
        }
        
        protected override LoadReturnCode OnLoad(ref string errorMessage)
        {
            
            return base.OnLoad(ref errorMessage);
        }

        public override PlugInLoadTime LoadTime => PlugInLoadTime.AtStartup;

        private void OnEndOpenDocument(object sender, DocumentOpenEventArgs e)
        {
            var objs = e.Document.Objects;
            foreach (var item in objs)
            {
                if (item is BrepObject brep)
                {
                    var userdata = brep.BrepGeometry.UserData.Find(typeof(OsmString)) as OsmString;
                    if (null == userdata)
                    {
                        continue;
                    }
                    var isSrf = brep.BrepGeometry.IsSurface;
                    var isSolid = brep.BrepGeometry.IsSolid;
                    if (isSolid)
                    {
                       
                        var space = new RHIB_Space(brep.BrepGeometry);
                        objs.Replace(new ObjRef(brep), space);
                    }
                }
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
            //if (layerIndex < 0 ) return false;

            //var objAtt = new ObjectAttributes { LayerIndex = layerIndex };
            //var glzObjAtt = new ObjectAttributes { LayerIndex = glzLayerIdx };


            doc.AdjustModelUnitSystem(UnitSystem.Meters, false);

            if (filename.Contains(".osm"))
            {

                var p = OPS.OpenStudioUtilitiesCore.toPath(filename);
                var tempModel = OPS.Model.load(p);
                if (!tempModel.is_initialized())
                {
                    return false;
                }
                var model = tempModel.get();
                var sps = model.getSpaces();


                //var tol = doc.ModelAbsoluteTolerance;
                Rhino.UI.Dialogs.ShowMessage(sps.Count + " space loaded", "test");

                foreach (OPS.Space sp in sps)
                {
                    //if (sp.nameString() != "Classroom_27_space")
                    //{
                    //    continue;
                    //}
                    var (space, glzs) = RHIB_Space.FromOpsSpace(sp);

                    //add glz surfaces to rhino doc.
                    foreach (var glz in glzs)
                    {
                        doc.Objects.AddBrep(glz);
                    }


                    //doc.Objects.AddBrep(space);
                    //var RHIB_obj = new RHIB_Space(space);

                    doc.Objects.AddRhinoObject(space); // TODO: this doesn't make sense!!!
                    space.Attributes.LayerIndex = layerIndex;
                    space.CommitChanges();



                }

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
        
        
    }
}