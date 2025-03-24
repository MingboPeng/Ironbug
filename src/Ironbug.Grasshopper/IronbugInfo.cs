using System;
using System.Drawing;
using Grasshopper.Kernel;
using GH = Grasshopper;
using Ironbug.HVAC;
using Rhino.UI;

namespace Ironbug.Grasshopper.Component
{
    public class IronbugInfo : GH_AssemblyInfo
    {
        public override string Name => "Ironbug.Grasshopper";
        public override Bitmap Icon => Properties.Resources.Ironbug;

        public override string Description => "A set of tools for HVAC energy modeling.";
        public override Guid Id => new Guid("af71b7c3-47f5-40b4-bb70-1b2fa1dd51e2");

        public override string AuthorName => "Mingbo Peng";
        public override string AuthorContact => "Mingbo@alumni.upenn.edu";
        //public override string AssemblyVersion => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        
        public override GH_LibraryLicense License => GH_LibraryLicense.opensource;
        public static string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }

    public class CategoryIcon : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            try
            {
                GH.Instances.DocumentServer.DocumentAdded += DocumentServer_DocumentAdded;
                Action<string> logger = (string message) => Rhino.RhinoApp.WriteLine($"Ironbug {IronbugInfo.version} is loaded with {message}");
                Core.OpenStudio.OpenStudioHelper.LoadAssemblies(logger);
            }
            catch (System.IO.FileNotFoundException loadError)
            {
                System.Windows.Forms.MessageBox.Show(loadError.Message);
            }

            GH.Instances.ComponentServer.AddCategoryIcon("Ironbug", Properties.Resources.Ironbug);
            GH.Instances.ComponentServer.AddCategoryShortName("Ironbug", "IB");
            GH.Instances.ComponentServer.AddCategorySymbolName("Ironbug", 'I');
            
            return GH_LoadingInstruction.Proceed;
        }

        private void DocumentServer_DocumentAdded(GH_DocumentServer sender, GH_Document doc)
        {
            if (doc != null)
            {
                //Rhino.RhinoApp.WriteLine($"Document added. Doc Name: {doc.DisplayName} {System.DateTime.Now}");
                // Hook into the document events
                doc.SolutionStart += OnSolutionStart;
                doc.SolutionEnd += Doc_SolutionEnd;
            }
        }

        private void Doc_SolutionEnd(object sender, GH_SolutionEventArgs e)
        {
            if (e.Document is GH_Document doc)
            {
                doc.SolutionEnd -= Doc_SolutionEnd;
                // Detect the start of document loading process
                //Rhino.RhinoApp.WriteLine($"Document is Ending its solution. Doc Name: {doc.DisplayName} {System.DateTime.Now}");
                IB_Utility.SkipComponentLevelCheck = false;

            }
        }

        private void OnSolutionStart(object sender, GH_SolutionEventArgs e)
        {
         
            if (e.Document is GH_Document doc)
            {
                doc.SolutionStart -= OnSolutionStart;
                // Detect the start of document loading process
                //Rhino.RhinoApp.WriteLine($"Document is starting its solution. Doc Name: {doc.DisplayName} {System.DateTime.Now}");
                IB_Utility.SkipComponentLevelCheck = true;
            }
        }

    }
}
