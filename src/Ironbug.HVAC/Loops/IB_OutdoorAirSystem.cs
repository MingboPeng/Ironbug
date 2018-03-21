using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_OutdoorAirSystem : IB_HVACObject, IIB_AirLoopObject
    {
        //include Controller inside the AirLoopHVACOutdoorAirSystem
        public IB_ControllerOutdoorAir IB_ControllerOutdoorAir { get; private set; } = new IB_ControllerOutdoorAir();
        
        private static AirLoopHVACOutdoorAirSystem InitMethod(Model model) => new AirLoopHVACOutdoorAirSystem(model, new ControllerOutdoorAir(model));

        
        
        public IB_OutdoorAirSystem():base(InitMethod(new Model()))
        {
        }
        

        public void AddController(IB_ControllerOutdoorAir ControllerOutdoorAir)
        {
            this.IB_ControllerOutdoorAir = ControllerOutdoorAir;
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((AirLoopHVACOutdoorAirSystem)this.ToOS(model)).addToNode(node);
        }

        

        private AirLoopHVACOutdoorAirSystem InitObjWithChild(Model model)
        {
            //Take child member to target model first
            var ctrl = (ControllerOutdoorAir)this.IB_ControllerOutdoorAir.ToOS(model);
            //init self with child member in target model.
            return new AirLoopHVACOutdoorAirSystem(model, ctrl);
        }
        public override ModelObject ToOS(Model model)
        {
            return (AirLoopHVACOutdoorAirSystem)base.ToOS(InitObjWithChild, model);
        }
        

        public override IB_ModelObject Duplicate()
        {
            //Duplicate self;
            var newObj = (IB_OutdoorAirSystem)base.DuplicateIBObj(() => new IB_OutdoorAirSystem());

            //Duplicate child member;
            var newCtrl = (IB_ControllerOutdoorAir)this.IB_ControllerOutdoorAir.Duplicate();

            //add new child member to new object;
            newObj.AddController(newCtrl);

            return newObj;
        }
    }
}
