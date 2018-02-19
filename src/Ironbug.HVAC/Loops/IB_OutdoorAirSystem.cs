using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_OutdoorAirSystem : IB_HVACComponent
    {
        //include Controller inside the AirLoopHVACOutdoorAirSystem
        public IB_ControllerOutdoorAir IB_ControllerOutdoorAir { get; private set; } = new IB_ControllerOutdoorAir();
        
        private static AirLoopHVACOutdoorAirSystem InitMethod(Model model) => new AirLoopHVACOutdoorAirSystem(model, new ControllerOutdoorAir(model));

        private AirLoopHVACOutdoorAirSystem InitMethodwithCtrl(Model model)
        {
            var ctrl = (ControllerOutdoorAir)this.IB_ControllerOutdoorAir.ToOS(model);
            return new AirLoopHVACOutdoorAirSystem(model, ctrl);
        }
        
        public IB_OutdoorAirSystem():base(InitMethod(new Model()))
        {
            base.SetName("AirLoopHVAC:OutdoorAirSystem");
        }

        public IB_OutdoorAirSystem(IB_ControllerOutdoorAir IB_Controller):this()
        {
            this.IB_ControllerOutdoorAir = IB_Controller;
        }

        public void AddController(IB_ControllerOutdoorAir ControllerOutdoorAir)
        {
            this.IB_ControllerOutdoorAir = ControllerOutdoorAir;
        }

        public override bool AddToNode(Model model, Node node)
        {
            
            return ((AirLoopHVACOutdoorAirSystem)this.ToOS(model)).addToNode(node);
        }
        
        public override ParentObject ToOS(Model model)
        {
            return (AirLoopHVACOutdoorAirSystem)base.ToOS(InitMethodwithCtrl, model);
        }
        
        public override IB_ModelObject Duplicate()
        {
            var newCtrl = (IB_ControllerOutdoorAir)this.IB_ControllerOutdoorAir.Duplicate();
            
            return base.Duplicate(() => new IB_OutdoorAirSystem(newCtrl));
        }
    }
}
