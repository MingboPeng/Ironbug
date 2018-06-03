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
        private static AirLoopHVACOutdoorAirSystem InitMethod(Model model) 
            => new AirLoopHVACOutdoorAirSystem(model, new ControllerOutdoorAir(model));
        private IB_Child IB_ControllerOutdoorAir => this.Children.GetChild<IB_ControllerOutdoorAir>();

        //TODO: finish this later
        private IList<IIB_AirLoopObject> OAStreamObjs = new List<IIB_AirLoopObject>();
        private IList<IIB_AirLoopObject> ReliefStreamObjs = new List<IIB_AirLoopObject>();

        
        public IB_OutdoorAirSystem():base(InitMethod(new Model()))
        {
            var controller = new IB_Child(new IB_ControllerOutdoorAir(), (obj) => this.SetController(obj as IB_ControllerOutdoorAir));
            this.Children.Add(controller);
            
        }
        
        public void SetHeatExchanger(IB_HeatExchangerAirToAirSensibleAndLatent heatExchanger)
        {
            this.OAStreamObjs.Add(heatExchanger);
        }

        public void SetController(IB_ControllerOutdoorAir ControllerOutdoorAir)
        {
            this.IB_ControllerOutdoorAir.Set(ControllerOutdoorAir);
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
                ((HVACComponent)item.ToOS(model)).addToNode(oaNode);
            };
            return true;
        }
        
        protected override ModelObject InitOpsObj(Model model)
        {
            var newObj = base.OnInitOpsObj(InitMethod, model).to_AirLoopHVACOutdoorAirSystem().get();
            newObj.setControllerOutdoorAir((ControllerOutdoorAir)this.IB_ControllerOutdoorAir.To<IB_ControllerOutdoorAir>().ToOS(model));

            return newObj;
        }
        

        public override IB_ModelObject Duplicate()
        {
            //Duplicate self;
            var newObj = (IB_OutdoorAirSystem)base.DuplicateIBObj(() => new IB_OutdoorAirSystem());

            //Duplicate child member;
            var newCtrl = (IB_ControllerOutdoorAir)this.IB_ControllerOutdoorAir.DuplicateChild();

            //add new child member to new object;
            newObj.SetController(newCtrl);

            return newObj;
        }

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
