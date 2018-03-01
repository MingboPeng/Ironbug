using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ironbug.HVAC
{
    public static class AirLoopHVAC_Extensions
    {
        //public static void CloneTo(this AirLoopHVAC fromLoop, Model model)
        //{
        //    var toLoop = new AirLoopHVAC(model);
        //    //copy loop settings
        //    toLoop.setName("_" + fromLoop.nameString());

        //    //copy loop components
        //    var spComs = fromLoop.supplyComponents().Where(_=>_.iddObjectType().valueDescription()!= "OS:Node");
        //    foreach (var item in spComs)
        //    {
        //        var type = item.OSType();
        //        var node = toLoop.supplyOutletNode();


        //        switch (type)
        //        {
        //            //copy outdoor air system
        //            case "OS:AirLoopHVAC:OutdoorAirSystem":
        //                var oa = item.to_AirLoopHVACOutdoorAirSystem().get().CloneTo(model);
        //                oa.addToNode(node);
        //                break;
        //            default:
        //                var hvacObj = item.clone(model).to_HVACComponent().get();
        //                hvacObj.addToNode(node);
        //                break;
        //        }
                


        //    }
            
        //    //copy sizingPlant settings
        //    //var sizing = fromPlant.sizingPlant().CloneTo(model, plantLoop);

        //    //copy setpoint Managers
        //    //var nd = toLoop.supplyOutletNode();
        //    //var sp = new OpenStudio.SetpointManagerOutdoorAirPretreat(model);
        //    //var ok = sp.addToNode(nd);
        //    //var sps = fromLoop.SetPointManagers();
        //    var ok = fromLoop.CloneSetPointManagers(toLoop);
            
        //}

        public static Dictionary<string, SetpointManager> SetPointManagersWithNodeName(this AirLoopHVAC fromLoop)
        {
            var node_spm = new Dictionary<string, SetpointManager>();

            var sps = fromLoop.SetPointManagers();

            foreach (var sp in sps)
            {
                Node nd = sp.setpointNode().get();
                node_spm.Add(nd.nameString(), sp);
            }

            return node_spm;
        }

        public static IEnumerable<SetpointManager> SetPointManagers(this AirLoopHVAC fromLoop)
        {
            var sps = fromLoop.model().getSetpointManagers()
                        .Where(_ =>
                            _.loop().get().EqualEqual(fromLoop)
                         );
            
            return sps;
        }

        //public static bool CloneSetPointManagers(this AirLoopHVAC fromLoop, AirLoopHVAC toLoop)
        //{
        //    var tModel = toLoop.model();
        //    var sps = fromLoop.SetPointManagers();

        //    //TODO: add checking entire loop. now only checks supply part.
        //    var loopSplyComsNames = fromLoop.supplyComponents().Select(_ => _.nameString());
        //    var loopOASys = fromLoop.airLoopHVACOutdoorAirSystem();
        //    var loopOASysComsNames = !loopOASys.isNull()? loopOASys.get().components().Select(_ => _.nameString()):new List<string>();

        //    //var loopNdWithSp = loopSplyComsNames.Select(_=>sps.ContainsKey(_)?)
        //    foreach (var sp in sps)
        //    {
        //        if (loopSplyComsNames.Contains(sp.Key))
        //        {
        //            int spAtIndex = loopSplyComsNames.ToList().IndexOf(sp.Key);

        //            //add setpoint to the targe loop at same index
        //            var nd = toLoop.supplyComponents()[spAtIndex].to_Node().get();
        //            var cloned = sp.Value.clone(tModel).to_SetpointManager().get().addToNode(nd);
                    
        //        }
        //        else if(loopOASysComsNames.Contains(sp.Key))
        //        {
        //            int spAtIndex = loopOASysComsNames.ToList().IndexOf(sp.Key);
        //            //var tLoopOaSys = toLoop.airLoopHVACOutdoorAirSystem();
        //            var tLoopOaSysComs = toLoop.airLoopHVACOutdoorAirSystem().get().components();
                    
        //            var nd = tLoopOaSysComs[spAtIndex].to_Node().get();
        //            var cloned = sp.Value.clone(tModel).to_SetpointManager().get().addToNode(nd);
                    
        //        }
        //    }

        //    var allCopied = toLoop.SetPointManagers().Count == sps.Count;
        //    return allCopied;

            
        //}
        

        
    }
}
