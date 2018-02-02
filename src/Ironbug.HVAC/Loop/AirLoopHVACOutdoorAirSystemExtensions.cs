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
            reComs.Reverse();
            var oaComs = fromOASys.oaComponents();

            for (int i = 0; i < reComs.Count; i++)
            {
                var currentCom = reComs[i];
                if (!currentCom.IsNode())
                {
                    var sameCom = currentCom.EqualEqual(oaComs[i]);
                    var reCom = currentCom.clone(model).to_HVACComponent().get();
                    var oaCom = oaComs[i].clone(model).to_HVACComponent().get();

                    var reNode = oa.outboardReliefNode().get();
                    var inNode = oa.outboardOANode().get();

                    if (sameCom)
                    {
                        reCom.addToNode(reNode);
                    }
                    else
                    {
                        reCom.addToNode(reNode);
                        oaCom.addToNode(inNode);
                    }
                }
                

            }

            return oa;

        }
    }
}
