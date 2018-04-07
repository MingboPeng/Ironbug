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

        protected (IEnumerable<IB_HVACObject> before, IB_LoopBranches branch, IEnumerable<IB_HVACObject> after) GetObjsBeforeAndAfterBranch(IEnumerable<IB_HVACObject> SupplyOrDemandObjs)
        {
            int branchIndex = SupplyOrDemandObjs.ToList().FindIndex(_ => _ is IB_LoopBranches);

            branchIndex = branchIndex == -1 ? SupplyOrDemandObjs.Count() : branchIndex;
            var beforeBranch = SupplyOrDemandObjs.Take(branchIndex);
            var afterBranch = SupplyOrDemandObjs.Skip(branchIndex+1);

            IB_LoopBranches branch = SupplyOrDemandObjs.OfType<IB_LoopBranches>().FirstOrDefault();

            return (beforeBranch, branch, afterBranch);

        }

        //protected IEnumerable<IB_HVACObject> GetObjsBeforeBranch(IEnumerable<IB_HVACObject> SupplyOrDemandObjs)
        //{
        //    int branchIndex = SupplyOrDemandObjs.ToList().FindIndex(_ => _ is IB_LoopBranches);

        //    //TODO: check the branch index (<0, or 0)
        //    var beforeBranch = SupplyOrDemandObjs.Take(branchIndex);
        //    ////SupplyOrDemandObjs.RemoveRange(0, branchIndex);

        //    //var newList = new List<IB_HVACObject>();
        //    //newList.AddRange(beforeBranch.Reverse());
        //    //newList.AddRange(SupplyOrDemandObjs.Skip(branchIndex));

        //    return beforeBranch.Reverse();

        //}

        //protected IEnumerable<IB_HVACObject> GetObjsAfterBranch(IEnumerable<IB_HVACObject> SupplyOrDemandObjs)
        //{
        //    int branchIndex = SupplyOrDemandObjs.ToList().FindIndex(_ => _ is IB_LoopBranches);

        //    return SupplyOrDemandObjs.Skip(branchIndex);

        //}

        protected bool AddSetPoints(Loop Loop, IEnumerable<IB_HVACObject> Components)
        {
            //var setPtAtIndex = new Dictionary<int, IB_SetpointManager>();

            //var trackingIDAndComp = new Dictionary<string, ModelObject>();


            var allTrackingIDs = Loop.components().Select(_ => _.comment()).ToList();
            foreach (var item in Components)
            {
                var aa = item is IB_SetpointManager;
            }

            var setPts = Components.Where(_ => _ is IB_SetpointManager);

            //TODO: check if there is only one component and it is setpoint.

            int added = 0;
            foreach (var item in setPts)
            {
                var setPt = (IB_SetpointManager)item;
                var atIndex = Components.ToList().IndexOf(item);

                OptionalNode nodeWithSetPt = null;
                
                if (atIndex == 0)
                {
                    //Find the component after setpoint
                    var comaAfterSetPt = Components.ElementAt(atIndex + 1).GetTrackingID();
                    var comaAfterSetPt_Index = allTrackingIDs.IndexOf(comaAfterSetPt);

                    //Find the node for setPoint
                    var node_Index = comaAfterSetPt_Index - 1;
                    nodeWithSetPt = Loop.components().ElementAt(node_Index).to_Node();
                }
                else if (atIndex > 0)
                {
                    //Find the component before setpoint
                    var comBeforeSetPt = Components.ElementAt(atIndex - 1).GetTrackingID();
                    var combeforeSetPt_Index = allTrackingIDs.IndexOf(comBeforeSetPt);

                    //Find the node for setPoint
                    var node_Index = combeforeSetPt_Index + 1;
                    nodeWithSetPt = Loop.components().ElementAt(node_Index).to_Node();
                }

                
                //Add to the node
                if (nodeWithSetPt.is_initialized())
                {
                    added = item.AddToNode(nodeWithSetPt.get()) ? added + 1:added;
                }
                
            }
            

            var allcopied = added == setPts.Count();

            if (!allcopied)
            {
                throw new Exception("Failed to add all set point managers!");
            }

            return allcopied;



        }
    }
}
