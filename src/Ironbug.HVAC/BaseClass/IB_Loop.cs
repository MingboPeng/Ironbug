using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_Loop : IB_ModelObject
    {
        public IB_Loop(ModelObject GhostOSObject) : base(GhostOSObject)
        {
        }

        protected (IEnumerable<IB_HVACObject> before, IB_LoopBranches branch, IEnumerable<IB_HVACObject> after) GetObjsBeforeAndAfterBranch(IEnumerable<IB_HVACObject> SupplyOrDemandObjs)
        {
            int branchIndex = SupplyOrDemandObjs.ToList().FindIndex(_ => _ is IB_LoopBranches);

            branchIndex = branchIndex == -1 ? SupplyOrDemandObjs.Count() : branchIndex;
            var beforeBranch = SupplyOrDemandObjs.Take(branchIndex);
            var afterBranch = SupplyOrDemandObjs.Skip(branchIndex + 1);

            IB_LoopBranches branch = SupplyOrDemandObjs.OfType<IB_LoopBranches>().FirstOrDefault();

            return (beforeBranch, branch, afterBranch);
        }

        public virtual ModelObject ToOS(Model model)
        {
            return this.NewOpsObj(model);
        }

        protected bool AddSetPoints(Node startingNode, IEnumerable<IB_HVACObject> components)
        {
            var Loop = startingNode.loop().get();
            var currentComps = Loop.components();
            var allTrackingIDs = currentComps.Select(_ => _.comment()).ToList();

            var setPts = components.Where(_ => _ is IB_SetpointManager);

            //check if there is set point
            if (setPts.Count() == 0)
            {
                return true;
            }

            int added = 0;

            //check if there is only one component and it is set point.
            if (setPts.Count() == components.Count())
            {
                foreach (var item in setPts)
                {
                    added = item.AddToNode(startingNode) ? added + 1 : added;
                }
                return true;
            }

            //check if set point is at the first
            IEnumerable<IB_HVACObject> remainingSetPts = null;
            if (components.First() is IB_SetpointManager)
            {
                added = setPts.First().AddToNode(startingNode) ? added + 1 : added;
                remainingSetPts = setPts.Skip(1);
            }
            else
            {
                remainingSetPts = setPts;
            }

            //until now, set point can only be at middle or the last
            foreach (var item in remainingSetPts)
            {
                //var setPt = (IB_SetpointManager)item;
                var atIndex = components.ToList().IndexOf(item);

                OptionalNode nodeWithSetPt = null;

                //Find the component before setpoint
                var comBeforeSetPt = components.ElementAt(atIndex - 1);
                var names = currentComps.Select(_ => _.nameString()).ToList();
                var nodeName = startingNode.nameString();
                var indexOfStartingNode = names.IndexOf(nodeName);
                //TODO: check if there is loop branch
                if (comBeforeSetPt is IB_LoopBranches)
                {
                    //search the first loop mixer after starting node
                    var searchList = currentComps.Skip(indexOfStartingNode);
                    var types = searchList.Select(_ => _.iddObjectType().valueName()).ToList();
                    var mixer = searchList.First(_ => _.iddObjectType().valueName() == "OS_Connector_Mixer").to_Mixer().get();

                    //get the first node after mixer
                    //var realMixer = mixer as  ConnectorMixer;
                    nodeWithSetPt = mixer.outletModelObject().get().to_Node();
                }
                else
                {
                    var comBeforeSetPt_ID = comBeforeSetPt.GetTrackingID();
                    var combeforeSetPt_Index = allTrackingIDs.IndexOf(comBeforeSetPt_ID);

                    //Find the node for setPoint
                    var node_Index = indexOfStartingNode + combeforeSetPt_Index + 1;
                    nodeWithSetPt = currentComps.ElementAt(node_Index).to_Node();
                }

                //Add to the node
                added = item.AddToNode(nodeWithSetPt.get()) ? added + 1 : added;
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