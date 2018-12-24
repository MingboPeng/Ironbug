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
            go.SetCommandPrompt("Select a space or a space's surface");
            go.GeometryFilter = ObjectType.Brep;
            go.SubObjectSelect = true;
            go.Get();
            if (go.CommandResult() != Result.Success)
                return go.CommandResult();

            var selObj = go.Object(0);
            var possibleSrf = selObj.Face();
            //var index = selObj.GeometryComponentIndex;
            var brepobj = selObj.Object() as BrepObject;

            var msgString = string.Empty;
            if (possibleSrf != null && brepobj is RHIB_Space)
            {

                var userdata = possibleSrf.UnderlyingSurface().UserData.Find(typeof(OsmString)) as OsmString;
                if (!string.IsNullOrWhiteSpace(userdata.Notes))
                {
                    msgString = userdata.Notes;
                }
            }
            else if (brepobj is RHIB_Space zone)
            {
                var userdata = zone.BrepGeometry.UserData.Find(typeof(OsmString)) as OsmString;
                if (!string.IsNullOrWhiteSpace(userdata.Notes))
                {
                    msgString = userdata.Notes;
                }
            }
            else if (brepobj is RHIB_SubSurface subSurface)
            {
                var userdata = subSurface.BrepGeometry.Surfaces[0].UserData.Find(typeof(OsmString)) as OsmString;
                if (!string.IsNullOrWhiteSpace(userdata.Notes))
                {
                    msgString = userdata.Notes;
                }
            }
            else
            {
                Rhino.UI.Dialogs.ShowMessage("Invalid OpenStudio object", "OpengStudio Info");
                return Result.Failure;
            }

            Rhino.UI.Dialogs.ShowMessage(msgString, "OpengStudio Info");
            
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
