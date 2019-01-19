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
            foreach (var item in oaObjs)
            {
                item.ToOS(model).addToNode(oaNode);
            };
            return true;
        }
        
        protected override ModelObject NewOpsObj(Model model)
        {
            var ctrl = (ControllerOutdoorAir)this.ControllerOutdoorAir.ToOS(model);
            var newObj = base.OnNewOpsObj((m)=>new AirLoopHVACOutdoorAirSystem(m,ctrl), model).to_AirLoopHVACOutdoorAirSystem().get();

            return newObj;
        }
        

        //public override IB_ModelObject Duplicate()
        //{
        //    //Duplicate self;
        //    var newObj = base.DuplicateIBObj(() => new IB_OutdoorAirSystem());

        //    //Duplicate child member;
        //    var newCtrl = (IB_ControllerOutdoorAir)this.ControllerOutdoorAir.Duplicate();

        //    //add new child member to new object;
        //    newObj.SetController(newCtrl);

        //    return newObj;
        //}

        //public override ModelObject ToOS(Model model)
        //{
        //    return base.ToOS(model);
        //}
        //public void ToOS_Demand(Loop Loop)
        //{
        //    this.ToOS()
        //}
    }
}
