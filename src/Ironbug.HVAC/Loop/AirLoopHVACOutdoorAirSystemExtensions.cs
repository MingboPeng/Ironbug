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

            var setpointManagers = fromOASys.airLoop().get().SetPointManagers();
            
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
                //else 
                //{
                //    var reNd = currentReCom.to_Node();
                //    var oaNd = currentOaCom.to_Node();

                //    if (!reNd.isNull())
                //    {
                //        var reName = reNd.get().nameString();
                //        if (setpointManagers.ContainsKey(reName))
                //        {
                //            var sp = setpointManagers[reName].clone(model).to_SetpointManager().get();
                //            sp.addToNode(reNode);
                //        }
                        
                //    }

                //    if (!oaNd.isNull())
                //    {
                //        var oaName = oaNd.get().nameString();
                //        if (setpointManagers.ContainsKey(oaName))
                //        {
                //            //var sp = setpointManagers[oaName].clone(model).to_SetpointManager().get();
                //            var sp = new SetpointManagerFollowOutdoorAirTemperature(model);
                //            var node = oa.oaComponents()
                //            var name = oaNode.nameString();
                //            oaNode.addSetpointManager(sp);
                //            //var ok = sp.addToNode(oaNode);
                //        }
                //    }
                //}
                

            }

            return oa;

        }

        //public static Dictionary<string, SetpointManager> SetPointManagers(this AirLoopHVACOutdoorAirSystem fromOASys)
        //{
        //    var sps = fromOASys.airLoop().get().SetPointManagers();
        //    var nds = fromOASys.components().Where(_ => _.IsNode()).Select(_=>_.nameString());

        //    var node_spm = new Dictionary<string, SetpointManager>();

        //    foreach (var nd in nds)
        //    {
        //        if (sps.ContainsKey(nd))
        //        {
        //            node_spm.Add(nd, sps[nd]);
        //        }
        //    }
            

        //    return node_spm;
        //}
    }
}
