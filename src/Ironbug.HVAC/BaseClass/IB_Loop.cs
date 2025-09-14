using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Ironbug.Core;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_Loop : IB_ModelObject, IEquatable<IB_Loop>
    {
        [DataMember]
        public List<IB_HVACObject> SupplyComponents { get; set; } = new List<IB_HVACObject>();
        [DataMember]
        public List<IB_HVACObject> DemandComponents { get; set; } = new List<IB_HVACObject>();
        public IB_Loop(Func<OpenStudio.Model, ModelObject> ghostObjInit) : base(ghostObjInit)
        {
        }

        #region Serialization
        public bool ShouldSerializesupplyComponents() => !this.SupplyComponents.IsNullOrEmpty();
        public bool ShouldSerializedemandComponents() => !this.DemandComponents.IsNullOrEmpty();
        #endregion

        protected IB_LoopBranches GetObjsBeforeAndAfterBranch(IEnumerable<IB_HVACObject> SupplyOrDemandObjs, out IEnumerable<IB_HVACObject> beforeBranch, out IEnumerable<IB_HVACObject> afterBranch)
        {
            int branchIndex = SupplyOrDemandObjs.ToList().FindIndex(_ => _ is IB_LoopBranches);

            branchIndex = branchIndex == -1 ? SupplyOrDemandObjs.Count() : branchIndex;
            beforeBranch = SupplyOrDemandObjs.Take(branchIndex);
            afterBranch = SupplyOrDemandObjs.Skip(branchIndex + 1);

            IB_LoopBranches branch = SupplyOrDemandObjs.OfType<IB_LoopBranches>().FirstOrDefault();

            return branch;
        }

        public override IB_ModelObject Duplicate()
        {
            var newObj = base.Duplicate() as IB_Loop;
            newObj.SupplyComponents = this.SupplyComponents.Select(_=>_.Duplicate() as IB_HVACObject).ToList();
            newObj.DemandComponents = this.DemandComponents.Select(_ => _.Duplicate() as IB_HVACObject).ToList();
            return newObj;

        }

        public abstract ModelObject ToOS(Model model);

        protected bool AddSetPoints(Model model, Node startingNode, IEnumerable<IB_HVACObject> Components)
        {
            var components = Components.Where(_ => !(_ is IB_NodeProbe));
            var setPts = components.OfType<IB_SetpointManager>();

            //check if there is set point
            if (setPts.Count() == 0) return true;

            var loop = startingNode.loop().get();
            var currentComps = loop.components();
            var allTrackingIDs = currentComps.Select(_ => _.comment()).ToList();



            int added = 0;

            //check if there is only one component and it is set point.
            if (setPts.Count() == components.Count())
            {
                foreach (var item in setPts)
                {
                    added = item.AddToNode(model, startingNode) ? added + 1 : added;
                }
                return true;
            }

            //check if set point is at the first
            IEnumerable<IB_HVACObject> remainingSetPts = null;
            if (components.First() is IB_SetpointManager)
            {
                added = setPts.First().AddToNode(model, startingNode) ? added + 1 : added;
                remainingSetPts = setPts.Skip(1);
            }
            else
            {
                remainingSetPts = setPts;
            }

            //until now, set point can only be at middle or the last
            foreach (var item in remainingSetPts)
            {
                OptionalNode nodeWithSetPt = null;
                //Find the component before setpoint
                var comBeforeSetPt = FindPreComponent(components, item);
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

                    if (combeforeSetPt_Index == -1)
                        throw new ArgumentException($"Failed to find object with id [{comBeforeSetPt_ID}] in loop");
                    //Find the node for setPoint
                    var node_Index = indexOfStartingNode + combeforeSetPt_Index + 1;
                    
                    nodeWithSetPt = currentComps.ElementAt(node_Index).to_Node();
                }

                //Add to the node
                added = item.AddToNode(model, nodeWithSetPt.get()) ? added + 1 : added;
            }

            var allcopied = added == setPts.Count();

            if (!allcopied)
            {
                throw new Exception("Failed to add all set point managers!");
            }

            return allcopied;

            IB_HVACObject FindPreComponent(IEnumerable<IB_HVACObject> Coms, IB_HVACObject currentItem)
            {
                var atIndex = Coms.ToList().IndexOf(currentItem);
                var preCom = Coms.ElementAt(atIndex - 1);
                if (preCom is IB_SetpointManager)
                {
                    return FindPreComponent(Coms, preCom);
                }
                else
                {
                    return preCom;
                }
            }

        }

        protected bool AddNodeProbe(Model model, Node startingNode, IEnumerable<IB_HVACObject> Components)
        {
            var components = Components.Where(_ => !(_ is IB_SetpointManager));
            var probes = components.Where(_ => _ is IB_NodeProbe).Select(_ => _ as IB_NodeProbe);
            //check if there is probes
            if (!probes.Any()) return true;

            var Loop = startingNode.loop().get();
            var currentComps = Loop.components();
            var allTrackingIDs = currentComps.Select(_ => _.comment()).ToList();

           

            int added = 0;

            var nodeName = startingNode.nameString();
            //check if there is only one component and it is probes.
            if (probes.Count() == components.Count())
            {
                foreach (var item in probes)
                {
                    startingNode.SetCustomAttributes(model, item.CustomAttributes);
                    startingNode.SetOutputVariables(model, item.CustomOutputVariables);

                    // node can be shared within model for availability manager's sensor input
                    OpsIDMapper.TryAdd(item.GetTrackingID(), startingNode.handle());

                    added++;
                }
                return true;
            }

            //check if probes is at the first
            IEnumerable<IB_NodeProbe> remainingProbes = null;
            if (components.First() is IB_NodeProbe)
            {
                var item = probes.First();
                startingNode.SetCustomAttributes(model, item.CustomAttributes);
                startingNode.SetOutputVariables(model, item.CustomOutputVariables);

                // node can be shared within model for availability manager's sensor input
                OpsIDMapper.TryAdd(item.GetTrackingID(), startingNode.handle());

                added = 1;

                remainingProbes = probes.Skip(1);
            }
            else
            {
                remainingProbes = probes;
            }

            //until now, probes can only be at middle or the last
            foreach (var item in remainingProbes)
            {
                var atIndex = components.ToList().IndexOf(item);

                OptionalNode nodeWithProbe = null;

                //Find the component before probes
                var comBeforeSetPt = components.ElementAt(atIndex - 1);
                var names = currentComps.Select(_ => _.nameString()).ToList();
       
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
                    nodeWithProbe = mixer.outletModelObject().get().to_Node();
                }
                else
                {
                    var comBeforeSetPt_ID = comBeforeSetPt.GetTrackingID();
                    var combeforeSetPt_Index = allTrackingIDs.IndexOf(comBeforeSetPt_ID);

                    //Find the node for setPoint
                    var node_Index = indexOfStartingNode + combeforeSetPt_Index + 1;
                    nodeWithProbe = currentComps.ElementAt(node_Index).to_Node();
                }

                //Add to the node
                var nd = nodeWithProbe.get();
                nd.SetCustomAttributes(model, item.CustomAttributes);
                nd.SetOutputVariables(model, item.CustomOutputVariables);

                // node can be shared within model for availability manager's sensor input
                OpsIDMapper.TryAdd(item.GetTrackingID(), nd.handle());

                //var ndName = nd.nameString();
                //AddOutputVariablesToModel(item.CustomOutputVariables, nd, model);
                added++;
            }

            var allcopied = added == probes.Count();

            if (!allcopied)
            {
                throw new Exception("Failed to add all node Probes!");
            }

            return allcopied;
        }

        public override bool Equals(object obj) => this.Equals(obj as IB_Loop);
        public bool Equals(IB_Loop obj)
        {
            if (!base.Equals(obj))
                return false;

            if (!this.DemandComponents.SequenceEqual(obj.DemandComponents)) 
                return false;

            if (!this.SupplyComponents.SequenceEqual(obj.SupplyComponents))
                return false;
    
            return true;
        }

    }
}