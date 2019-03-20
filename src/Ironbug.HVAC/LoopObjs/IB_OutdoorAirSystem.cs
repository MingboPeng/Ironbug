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

        //TODO: finish this later
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
            var model = node.model();
            var oa = ((AirLoopHVACOutdoorAirSystem)this.ToOS(model));
            oa.addToNode(node);
            var oaNode = oa.outboardOANode().get();
            var oaObjs = this.OAStreamObjs.Reverse();

            var comps = oaObjs.Where(_ => !(_ is IB_SetpointManager));
            foreach (var item in comps)
            {
                item.AddToNode(oaNode);
            };
            AddSetPoints(oaNode, oaObjs);

            var rfNode = oa.outboardReliefNode().get();
            var rfObjs = this.ReliefStreamObjs;
            foreach (var item in rfObjs)
            {
                item.ToOS(model).addToNode(rfNode);
            };

            return true;
        }

        public override HVACComponent ToOS(Model model)
        {
            var ctrl = (ControllerOutdoorAir)this.ControllerOutdoorAir.ToOS(model);
            var newObj = base.OnNewOpsObj((m) => new AirLoopHVACOutdoorAirSystem(m, ctrl), model);

            return newObj;
        }

        public override IB_HVACObject Duplicate()
        {
            //Duplicate self;
            var newObj = base.DuplicateIBObj(() => new IB_OutdoorAirSystem());

            //Duplicate child member;
            var newCtrl = (IB_ControllerOutdoorAir)this.ControllerOutdoorAir.Duplicate();

            //add new child member to new object;
            newObj.SetController(newCtrl);

            foreach (var item in this.ReliefStreamObjs)
            {
                newObj.ReliefStreamObjs.Add(item.Duplicate());
            };

            foreach (var item in this.OAStreamObjs)
            {
                newObj.OAStreamObjs.Add(item.Duplicate());
            };


            return newObj;
        }

        protected bool AddSetPoints(Node startingNode, IEnumerable<IB_HVACObject> components)
        {

            var Loop = startingNode.loop().get();
            
            var currentComps = Loop.to_AirLoopHVAC().get().airLoopHVACOutdoorAirSystem().get().oaComponents();
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
               
                var comBeforeSetPt_ID = comBeforeSetPt.GetTrackingID();
                
                var combeforeSetPt_Index = allTrackingIDs.IndexOf(comBeforeSetPt_ID);

                //Find the node for setPoint
                var node_Index = indexOfStartingNode + combeforeSetPt_Index - 1;
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
    }
}
