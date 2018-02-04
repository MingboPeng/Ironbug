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

        

        
    }
}
