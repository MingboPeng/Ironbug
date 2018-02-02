using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ironbug.HVAC
{
    public static class AirLoopHVACExtensions
    {
        public static void CloneTo(this AirLoopHVAC fromLoop, Model model)
        {
            var toLoop = new AirLoopHVAC(model);
            //copy loop settings
            toLoop.setName("_" + fromLoop.nameString());

            //copy loop components
            var spComs = fromLoop.supplyComponents().Where(_=>_.iddObjectType().valueDescription()!= "OS:Node");
            foreach (var item in spComs)
            {
                var type = item.OSType();
                var node = toLoop.supplyOutletNode();


                switch (type)
                {
                    case "OS:AirLoopHVAC:OutdoorAirSystem":
                        var oa = item.to_AirLoopHVACOutdoorAirSystem().get().CloneTo(model);
                        oa.addToNode(node);
                        break;
                    default:
                        var hvacObj = item.clone(model).to_HVACComponent().get();
                        hvacObj.addToNode(node);
                        break;
                }
                


            }

            //copy outdoor air system
            //var outAirSys = fromLoop.airLoopHVACOutdoorAirSystem();
            //if (!outAirSys.isNull())
            //{
            //    outAirSys.get().comp
            //}

            //copy sizingPlant settings
            //var sizing = fromPlant.sizingPlant().CloneTo(model, plantLoop);

            //copy setpoint Managers
            //var sps = fromLoop.SetPointManagers();
            
            //foreach (var item in sps)
            //{
            //    item.nod
            //}
        }

        public static IEnumerable<SetpointManager> SetPointManagers(this AirLoopHVAC fromLoop)
        {
            //var sps = fromLoop.model().getSetpointManagers()
            //            .Where(_ =>
            //                _.loop().get().handle().EqualEqual(fromLoop.handle())
            //            );

            var sps = fromLoop.model().getSetpointManagers()
                        .Where(_ =>
                            _.loop().get().EqualEqual(fromLoop)
                         );

            return sps;
        }

        public static AirLoopHVACOutdoorAirSystem CloneTo(this AirLoopHVACOutdoorAirSystem fromOASys, Model model)
        {
            var oa = fromOASys.clone(model).to_AirLoopHVACOutdoorAirSystem().get();
            var reliefComs= fromOASys.reliefComponents();
            reliefComs.Reverse();
            var inComs = fromOASys.oaComponents();

            for (int i = 0; i < reliefComs.Count; i++)
            {
                var reCom = reliefComs[i].to_HVACComponent().get();
                var inCom = inComs[i].to_HVACComponent().get();
                var reNode = oa.outboardReliefNode().get();
                var inNode = oa.outboardOANode().get();
                reCom.addToNode(reNode);
                inCom.addToNode(inNode);

            }

            return oa;
            
        }

        public static string OSType(this ModelObject component)
        {
            return component.iddObjectType().valueDescription();
        }
    }
}
