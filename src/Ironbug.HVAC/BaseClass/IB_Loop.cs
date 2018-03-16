using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_Loop : IB_ModelObject, IIB_ToOPSable
    {
        public IB_Loop(ModelObject GhostOSObject) : base(GhostOSObject)
        {
        }

        public abstract ModelObject ToOS(Model model);

        protected IEnumerable<IB_HVACObject> GetObjsBeforeBranch(List<IB_HVACObject> SupplyOrDemandObjs)
        {
            int branchIndex = SupplyOrDemandObjs.FindIndex(_ => _ is IB_LoopBranches);

            //TODO: check the branch index (<0, or 0)
            var beforeBranch = SupplyOrDemandObjs.Take(branchIndex);
            ////SupplyOrDemandObjs.RemoveRange(0, branchIndex);

            //var newList = new List<IB_HVACObject>();
            //newList.AddRange(beforeBranch.Reverse());
            //newList.AddRange(SupplyOrDemandObjs.Skip(branchIndex));

            return beforeBranch;

        }

        protected IEnumerable<IB_HVACObject> GetObjsAfterBranch(List<IB_HVACObject> SupplyOrDemandObjs)
        {
            int branchIndex = SupplyOrDemandObjs.FindIndex(_ => _ is IB_LoopBranches);

            return SupplyOrDemandObjs.Skip(branchIndex);

        }

        protected bool AddSetPoints(Loop Loop, List<IB_HVACObject> Components)
        {
            //var setPtAtIndex = new Dictionary<int, IB_SetpointManager>();

            //var trackingIDAndComp = new Dictionary<string, ModelObject>();


            var allTrackingIDs = Loop.components().Select(_ => _.comment()).ToList();


            var setPts = Components.Where(_ => _ is IB_SetpointManager);

            //TODO: check if there is only one component and it is setpoint.


            foreach (var item in setPts)
            {
                var setPt = (IB_SetpointManager)item;
                var atIndex = Components.IndexOf(item);

                OptionalNode nodeWithSetPt = null;

                if (atIndex == 0)
                {
                    //Find the component after setpoint
                    var comaAfterSetPt = Components[atIndex + 1].GetTrackingID();
                    var comaAfterSetPt_Index = allTrackingIDs.IndexOf(comaAfterSetPt);

                    //Find the node for setPoint
                    var node_Index = comaAfterSetPt_Index - 1;
                    nodeWithSetPt = Loop.components().ElementAt(node_Index).to_Node();
                }
                else if (atIndex > 0)
                {
                    //Find the component before setpoint
                    var comBeforeSetPt = Components[atIndex - 1].GetTrackingID();
                    var combeforeSetPt_Index = allTrackingIDs.IndexOf(comBeforeSetPt);

                    //Find the node for setPoint
                    var node_Index = combeforeSetPt_Index + 1;
                    nodeWithSetPt = Loop.components().ElementAt(node_Index).to_Node();
                }


                //Add to the node
                if (nodeWithSetPt.is_initialized())
                {
                    item.AddToNode(nodeWithSetPt.get());
                }



            }
            

            var allcopied = Loop.SetPointManagers().Count() == setPts.Count();

            if (!allcopied)
            {
                throw new Exception("Failed to add set point managers!");
            }

            return allcopied;



        }
    }
}
