using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Ironbug
{
    public class IronbugInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Ironbug";
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
}
