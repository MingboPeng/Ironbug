using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public interface IIB_ToOPSable
    {
         ModelObject ToOS(Model model);
    }
}
