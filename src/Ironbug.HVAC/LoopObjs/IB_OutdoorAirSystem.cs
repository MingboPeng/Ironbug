using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_OutdoorAirSystem : IB_HVACObject, IIB_AirLoopObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_OutdoorAirSystem();
        private static AirLoopHVACOutdoorAirSystem NewDefaultOpsObj(Model model) 
            => new AirLoopHVACOutdoorAirSystem(model, new ControllerOutdoorAir(model));
        private IB_ControllerOutdoorAir ControllerOutdoorAir => this.Children.Get<IB_ControllerOutdoorAir>();
        
        private IList<IB_HVACObject> OAStreamObjs = new List<IB_HVACObject>();
        private IList<IB_HVACObject> ReliefStreamObjs = new List<IB_HVACObject>();

        
        public IB_OutdoorAirSystem():base(NewDefaultOpsObj(new Model()))
        {

            this.AddChild(new IB_ControllerOutdoorAir());
            
        }

        public void AddToOAStream(IB_HVACObject Obj)
        {
            this.OAStreamObjs.Add(Obj);
        }

        public void AddToReliefStream(IB_HVACObject Obj)
        {
            this.ReliefStreamObjs.Add(Obj);
        }


        public void SetHeatExchanger(IB_HeatExchangerAirToAirSensibleAndLatent heatExchanger)
        {
            this.OAStreamObjs.Add(heatExchanger);
        }

        public void SetController(IB_ControllerOutdoorAir ControllerOutdoorAir)
        {
            this.SetChild(ControllerOutdoorAir);
        }

        public override bool AddToNode(Node node)
        {
            IB_HeatExchangerAirToAirSensibleAndLatent hx = null;
            if (this.OAStreamObjs.FirstOrDefault() is IB_HeatExchangerAirToAirSensibleAndLatent heatEx)
            {
                hx = heatEx;
            }

            var model = node.model();
            var oa = ((AirLoopHVACOutdoorAirSystem)this.ToOS(model));
            oa.addToNode(node);
            var oaNode = oa.outboardOANode().get();
            var oaObjs = this.OAStreamObjs.Reverse();

            var comps = oaObjs.Where(_ => !(_ is IB_SetpointManager) && !(_ is IB_Probe));
            foreach (var item in comps)
            {
                item.AddToNode(oaNode);
            };
            var osComs = oa.oaComponents();
            oaObjs = oaObjs.Reverse();
            AddSetPoints(oaObjs, osComs);
            AddNodeProbe(oaObjs, osComs);


            var rfNode = oa.outboardReliefNode().get();
            var rfObjs = this.ReliefStreamObjs.Reverse().ToList();
            var rfcomps = rfObjs.Where(_ => !(_ is IB_SetpointManager) && !(_ is IB_Probe));
            foreach (var item in rfcomps)
            {
                item.ToOS(model).addToNode(rfNode);
            };
            var reComs = oa.reliefComponents();
            if (hx != null)
            {
                rfObjs.Insert(0, hx);
            }
            AddSetPoints(rfObjs, reComs);
            AddNodeProbe(rfObjs, reComs);

            return true;
        }

        public override HVACComponent ToOS(Model model)
        {
            var ctrl = (ControllerOutdoorAir)this.ControllerOutdoorAir.ToOS(model);
            var newObj = base.OnNewOpsObj((m) => new AirLoopHVACOutdoorAirSystem(m, ctrl), model);

            return newObj;
        }

        public override IB_ModelObject Duplicate()
        {
            //Duplicate self;
            var newObj = base.Duplicate(() => new IB_OutdoorAirSystem());

            //Duplicate child member;
            var newCtrl = (IB_ControllerOutdoorAir)this.ControllerOutdoorAir.Duplicate();

            //add new child member to new object;
            newObj.SetController(newCtrl);

            foreach (var item in this.ReliefStreamObjs)
            {
                newObj.ReliefStreamObjs.Add(item.Duplicate() as IB_HVACObject);
            };

            foreach (var item in this.OAStreamObjs)
            {
                newObj.OAStreamObjs.Add(item.Duplicate() as IB_HVACObject);
            };


            return newObj;
        }

        protected bool AddSetPoints(IEnumerable<IB_HVACObject> Components, IEnumerable<ModelObject> CurrentAddedObj)
        {
            var components = Components.Where(_ => !(_ is IB_Probe));
            var setPts = components.Where(_ => _ is IB_SetpointManager).Select(_ => _ as IB_SetpointManager);
            //check if there is set point
            if (setPts.Count() == 0) return true;


            var currentComps = CurrentAddedObj;
            var firstNode = currentComps.First().to_Node().get();
            var model = firstNode.model();
            
            var allTrackingIDs = currentComps.Select(_ => _.comment()).ToList();

            
            

            int added = 0;

            //check if there is only one component and it is set point.
            if (setPts.Count() == components.Count())
            {
                foreach (var item in setPts)
                {
                    added = item.AddToNode(firstNode) ? added + 1 : added;
                }
                return true;
            }

            var cnames = currentComps.Select(_ => _.nameString()).ToList();
            //check if set point is at the first
            IEnumerable<IB_HVACObject> remainingSetPts = null;
            if (components.First() is IB_SetpointManager)
            {
                added = setPts.First().AddToNode(firstNode) ? added + 1 : added;
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
                var comBeforeSetPt_ID = comBeforeSetPt.GetTrackingID();
                var combeforeSetPt_Index = allTrackingIDs.IndexOf(comBeforeSetPt_ID);

                //Find the node for setPoint
                var node_Index = combeforeSetPt_Index + 1;
                nodeWithSetPt = currentComps.ElementAt(node_Index).to_Node();
              

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
        protected bool AddNodeProbe(IEnumerable<IB_HVACObject> Components, IEnumerable<ModelObject> CurrentAddedObj)
        {
            var components = Components.Where(_ => !(_ is IB_SetpointManager));
            var probes = components.Where(_ => _ is IB_Probe).Select(_ => _ as IB_Probe);
            //check if there is probes
            if (!probes.Any()) return true;
            
            var currentComps = CurrentAddedObj;
            var firstNode = currentComps.First().to_Node().get();
            var model = firstNode.model();
            
            var allTrackingIDs = currentComps.Select(_ => _.comment()).ToList();
            
            int added = 0;

            //var cnames = currentComps.Select(_ => _.nameString()).ToList();
            
            //check if there is only one component and it is probes.
            if (probes.Count() == components.Count())
            {
                foreach (var item in probes)
                {
                    AddProbeToNode(firstNode, item.CustomAttributes, item.CustomOutputVariables);
                    added++;
                }
                return true;
            }

            //check if probes is at the first
            IEnumerable<IB_Probe> remainingProbes = null;
            if (components.First() is IB_Probe)
            {
                var item = probes.First();
                AddProbeToNode(firstNode, item.CustomAttributes, item.CustomOutputVariables);
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
                var comBeforeSetPt_ID = comBeforeSetPt.GetTrackingID();
                var combeforeSetPt_Index = allTrackingIDs.IndexOf(comBeforeSetPt_ID);

                //Find the node for setPoint
                var node_Index = combeforeSetPt_Index +1;
                nodeWithProbe = currentComps.ElementAt(node_Index).to_Node();

                //Add to the node
                AddProbeToNode(nodeWithProbe.get(), item.CustomAttributes, item.CustomOutputVariables);
                added++;
            }

            var allcopied = added == probes.Count();

            if (!allcopied)
            {
                throw new Exception("Failed to add all node Probes in OutdoorAirSystem!");
            }

            return allcopied;

            bool AddProbeToNode(Node Node, Dictionary<IB_Field, object> CustomAttributes, List<IB_OutputVariable> CustomOutputVariables)
            {
                Node.SetCustomAttributes(CustomAttributes);
                return Node.SetOutputVariables(this.CustomOutputVariables);

            }
        }
    }
}
