using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    //temporary for Solar Collector Flat Plate Water, delete if not needed
    public abstract class IB_SolarColletorPerformanceFlatPlate : IB_HVACObject, IIB_PlantLoopObjects
    {

        public IB_SolarColletorPerformanceFlatPlate(HVACComponent GhostOSObject) : base(GhostOSObject)
        {

        }
    }
}
