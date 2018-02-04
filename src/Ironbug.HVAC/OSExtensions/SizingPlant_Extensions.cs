using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public static class SizingPlane_Extensions
    {
        public static SizingPlant CloneTo(this SizingPlant szPlant, Model model, PlantLoop plantLoop)
        {
            var s = new SizingPlant(model, plantLoop);

            s.setLoopType(szPlant.loopType());
            s.setLoopDesignTemperatureDifference(szPlant.loopDesignTemperatureDifference());
            s.setDesignLoopExitTemperature(szPlant.designLoopExitTemperature());

            return s;

        }
    }
    
}
