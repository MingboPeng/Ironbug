using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Ironbug.LBHB_Legacy
{
    public class IronbugLBHB_LegacyInfo : GH_AssemblyInfo
   {
        public override string Name => "Ironbug.LBHB_Legacy";
        public override Bitmap Icon => null;
        public override string Description => "Ironbug components for ladybug tools";
        public override Guid Id => new Guid("7a16f00b-ff8f-49c8-bdc6-7d6524ac872a");

        public override string AuthorName => "Mingbo Peng";
        public override string AuthorContact => "Mingbo@alumni.upenn.edu";
    }
}
