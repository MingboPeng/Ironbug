using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenStudio;

namespace Ironbug.HVAC
{
    public static class Extensions
    {
        

        public static void CloneTo (this PlantLoop fromPlant, Model model)
        {
            var plantLoop = new PlantLoop(model);
            plantLoop.setName("_" + fromPlant.nameString());

            //copy plant loop settings


            //copy sizingPlant settings
            var sizing = fromPlant.sizingPlant().CloneTo(model, plantLoop);
            
        }

        public static SizingPlant CloneTo(this SizingPlant szPlant, Model model, PlantLoop plantLoop)
        {
            var s = new SizingPlant(model, plantLoop);
            
            s.setLoopType(szPlant.loopType());
            s.setLoopDesignTemperatureDifference(szPlant.loopDesignTemperatureDifference());
            s.setDesignLoopExitTemperature(szPlant.designLoopExitTemperature());

            return s;
            
        }

        public static bool Save(this Model model, string filePath)
        {
            return model.save(new Path(filePath), true);

        }
    }
}
