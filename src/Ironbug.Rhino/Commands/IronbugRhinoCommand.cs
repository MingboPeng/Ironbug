using Rhino;
using Rhino.Commands;

namespace Ironbug.RhinoOpenStudio.Commands
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

    public class SaveOSM : Command
    {
        public SaveOSM()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static SaveOSM Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "SaveOSM"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            //var path = IronbugRhinoPlugIn.Instance.OsmFilePath;
            //var osmP = OpenStudio.OpenStudioUtilitiesCore.toPath(path);
            //var osmModel = OpenStudio.Model.load(osmP).get();

            //var go = new GetObject();
            //go.SetCommandPrompt("Select a glazing surface to update osm");
            //go.GeometryFilter = ObjectType.Brep;
            //go.SubObjectSelect = true;
            //go.Get();
            //if (go.CommandResult() != Result.Success)
            //    return go.CommandResult();

            //var selObj = go.Object(0);
            //var possibleSrf = selObj.Face();
            ////var index = selObj.GeometryComponentIndex;
            //var brepobj = selObj.Object() as BrepObject;

            //var msgString = string.Empty;
            //if (brepobj is RHIB_SubSurface subSurface)
            //{
            //    if (!subSurface.ToOS(osmModel))
            //        return Result.Failure;

            //    var testOsmFile = @"C:\Users\mingo\OneDrive\Desktop\TestFiles\20181212_MHS_PC_SavedFromRh.osm";
            //    var newOsmP = OpenStudio.OpenStudioUtilitiesCore.toPath(testOsmFile);
            //    var isSaved = osmModel.save(newOsmP, true);

            //    if (!isSaved)
            //        return Result.Failure;

            //    msgString = "saved to " + testOsmFile;
            //}
            //else
            //{
            //    Rhino.UI.Dialogs.ShowMessage("Invalid OpenStudio object", "OpengStudio Info");
            //    return Result.Failure;
            //}

            //Rhino.UI.Dialogs.ShowMessage(msgString, "OpengStudio Info");

            return Result.Success;
        }
    }
}