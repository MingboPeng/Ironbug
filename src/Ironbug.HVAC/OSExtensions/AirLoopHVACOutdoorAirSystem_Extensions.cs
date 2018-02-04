using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ironbug.HVAC
{
    public static class AirLoopHVACOutdoorAirSystemExtensions
    {
        public static AirLoopHVACOutdoorAirSystem CloneTo(this AirLoopHVACOutdoorAirSystem fromOASys, Model model)
        {
            var oa = fromOASys.clone(model).to_AirLoopHVACOutdoorAirSystem().get();
            var reComs = fromOASys.reliefComponents();
            var oaComs = fromOASys.oaComponents();
            oaComs.Reverse();
            
            
            for (int i = 0; i < reComs.Count; i++)
            {
                var currentReCom = reComs[i];
                var currentOaCom = oaComs[i];
                var reNode = oa.outboardReliefNode().get();
                var oaNode = oa.outboardOANode().get();


                if (!currentReCom.IsNode() || !currentOaCom.IsNode())
                {
                    var sameCom = currentReCom.EqualEqual(currentOaCom);
                    var reCom = currentReCom.clone(model).to_HVACComponent().get();
                    var oaCom = currentOaCom.clone(model).to_HVACComponent().get();
                    
                    if (sameCom)
                    {
                        reCom.addToNode(reNode);
                    }
                    else
                    {
                        reCom.addToNode(reNode);
                        oaCom.addToNode(oaNode);
                    }
                }
                

            }

            return oa;

        }

        
    }
}
