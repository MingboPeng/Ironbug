using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public interface IIB_DualLoopObjects:IIB_ToOPSable, IIB_AirLoopObject, IIB_PlantLoopObjects
    {
    }
}
