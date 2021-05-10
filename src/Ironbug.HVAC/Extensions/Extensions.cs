using Ironbug.HVAC.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public static class Extensions
    {
        public static int CountWithBranches(this IEnumerable<IB_HVACObject> enumerable)
        {
            var count = 0;
            foreach (var item in enumerable)
            {
                if (item is IB_PlantLoopBranches pb)
                {
                    count += pb.Count();
                }
                else if (item is IB_AirLoopBranches ab)
                {
                    foreach (var zb in ab.Branches)
                    {
                        var zone = zb[0] as IB_ThermalZone;
                        if (zone.AirTerminal is IB_AirTerminalSingleDuctInletSideMixer)
                        {
                            count += 3; // because added air terminal with each zone
                        }
                        else
                        {
                            count += 2; // because added air terminal with each zone
                        }
                    }
                    
                }
                else
                {
                    count++;
                }
            }

            return count;
        }

     
    }
}
