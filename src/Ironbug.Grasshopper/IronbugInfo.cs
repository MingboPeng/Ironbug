using System;
using System.Drawing;
using Grasshopper.Kernel;
using GH = Grasshopper;

namespace Ironbug.Grasshopper.Component
{
    public class IronbugInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Ironbug.Grasshopper";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("af71b7c3-47f5-40b4-bb70-1b2fa1dd51e2");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Mingbo Peng";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }

    public class CategoryIcon : GH_AssemblyPriority
    {
        public override GH_LoadingInstruction PriorityLoad()
        {
            try
            {
                var osmVersion = "2.7.0.0";
                Action<string> logger = (string message) => Rhino.RhinoApp.WriteLine(message);
                Core.OpenStudio.OpenStudioHelper.LoadAssemblies(logger, osmVersion);
            }
            catch (System.IO.FileNotFoundException loadError)
            {
                throw new ArgumentException(loadError.Message);
            }

            GH.Instances.ComponentServer.AddCategoryIcon("Ladybug", Properties.Resources.Ladybug);
            GH.Instances.ComponentServer.AddCategoryIcon("Honeybee", Properties.Resources.Honeybee);

            GH.Instances.ComponentServer.AddCategoryIcon("Ironbug", Properties.Resources.Ironbug);
            GH.Instances.ComponentServer.AddCategoryShortName("Ironbug", "IB");
            GH.Instances.ComponentServer.AddCategorySymbolName("Ironbug", 'I');
            
            return GH_LoadingInstruction.Proceed;
        }
    }
}
