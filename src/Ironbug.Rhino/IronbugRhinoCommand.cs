using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input;
using Rhino.Input.Custom;
using Ironbug.RhinoOpenStudio.GeometryConverter;

namespace Ironbug.RhinoOpenStudio
{
    public class IronbugRhinoCommand : Command
    {
        public IronbugRhinoCommand()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static IronbugRhinoCommand Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "IronbugRhinoCommand"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // Usually commands in import plug-ins are used to modify settings and behavior.
            // The import work itself is performed by the IronbugRhinoPlugIn class.
            
            return Result.Success;
        }
    }

    public class osmInfo : Command
    {
        public osmInfo()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static osmInfo Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "osmInfo"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var go = new GetObject();
            go.SetCommandPrompt("Select a zone or a surface");
            go.GeometryFilter = ObjectType.Brep;
            go.SubObjectSelect = true;
            go.Get();
            if (go.CommandResult() != Result.Success)
                return go.CommandResult();

            var selObj = go.Object(0);
            var possibleSrf = selObj.Face();
            //var index = selObj.GeometryComponentIndex;
            var brepobj = selObj.Object() as BrepObject;

            if (possibleSrf!=null)
            {
                //Rhino.UI.Dialogs.ShowMessage(possibleSrf.UserDictionary.GetString("OSM_String", string.Empty), "testsrfs");
                //var objbytes = possibleSrf.UserDictionary.GetBytes("OSM_Object");
                //var osmobj = ByteArrayToObject(objbytes) as OpenStudio.Space;
                //Rhino.UI.Dialogs.ShowMessage(osmobj.__str__(), "testsrfs");

                var userdata = possibleSrf.UnderlyingSurface().UserData.Find(typeof(OsmString)) as OsmString;
                Rhino.UI.Dialogs.ShowMessage(userdata.Notes, "testsrfs");
            }
            else if (brepobj is RHIB_Space zone)
            {
                Rhino.UI.Dialogs.ShowMessage(zone.BrepGeometry.UserDictionary.GetString("OSM_String", string.Empty), "test");
            }
            else
            {
                Rhino.UI.Dialogs.ShowMessage("Invalid OpenStudio geometry", "test");
                return Result.Failure;
            }
            
            

            //srf = brepobj.BrepGeometry.Faces[srf.FaceIndex];

            //ObjRef selZone;
            //Result rc = RhinoGet.GetOneObject("select one zone", false, Rhino.DocObjects.ObjectType.AnyObject, out selZone);
            //if (rc == Result.Success)
            //{
            //    if (selZone != null)
            //    {
            //        var obj = doc.Objects.FindId(selZone.ObjectId);
            //        if (obj is RHIB_Space zone)
            //        {
            //            Rhino.UI.Dialogs.ShowMessage(zone.UserDictionary.GetString("OSM_String",string.Empty), "test");
            //        }
            //        else
            //        {
            //            //Rhino.UI.Dialogs.ShowMessage(obj.UserDictionary.GetString("OSM_String", string.Empty), "test");
            //        }
                    

            //        //var osmzone = selZone.Geometry() as RHIB_ThermalZone;
            //        //osmzone.
            //    }


            //}

            return Result.Success;
        }

        

        //public static Object ByteArrayToObject(byte[] arrBytes)
        //{
        //    using (var memStream = new System.IO.MemoryStream())
        //    {
        //        var binForm = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        //        memStream.Write(arrBytes, 0, arrBytes.Length);
        //        memStream.Seek(0, System.IO.SeekOrigin.Begin);
        //        var obj = binForm.Deserialize(memStream);
        //        return obj;
        //    }
        //}
    }
}
