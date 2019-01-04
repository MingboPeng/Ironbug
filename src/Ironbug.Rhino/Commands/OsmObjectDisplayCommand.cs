using Rhino;
using Rhino.Commands;

namespace Ironbug.RhinoOpenStudio.Commands
{
    public class OsmObjectDisplayCommand : Command
    {
        public override string EnglishName => "ShowEnvelope";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            
            if (m_conduit != null)
            {
                m_conduit.Enabled = false;
                m_conduit = null;
                //RhinoDoc.ReplaceRhinoObject -= RhinoDoc_ReplaceRhinoObject;
            }
            else
            {
                m_conduit = new OsmObjDisplayConduit { Enabled = true };
                //RhinoDoc.ReplaceRhinoObject += RhinoDoc_ReplaceRhinoObject;

            }
            doc.Views.Redraw();
            return Result.Success;
        }

        //private void RhinoDoc_ReplaceRhinoObject(object sender, Rhino.DocObjects.RhinoReplaceObjectEventArgs e)
        //{
        //    RhinoApp.Write("Display View updated");
        //    m_conduit.Enabled = false;
        //    m_conduit = null;
        //    RhinoDoc.ActiveDoc.Views.Redraw();
        //    m_conduit = new OsmObjDisplayConduit { Enabled = true };
        //    RhinoDoc.ActiveDoc.Views.Redraw();
        //}

        private static OsmObjDisplayConduit m_conduit;
    }
}