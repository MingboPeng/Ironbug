using Rhino;
using Rhino.Commands;

namespace Ironbug.RhinoOpenStudio.Commands
{
    public class OsmObjectDisplayCommand : Command
    {
        public override string EnglishName => "OsmObjectDisplay";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            if (m_conduit != null)
            {
                m_conduit.Enabled = false;
                m_conduit = null;
            }
            else
            {
                m_conduit = new OsmObjDisplayConduit { Enabled = true };
            }
            doc.Views.Redraw();
            return Result.Success;
        }

        private static OsmObjDisplayConduit m_conduit;
    }
}