using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_OutdoorAirSystem : IB_HVACComponent
    {
        //Real obj to be saved in OS model
        //private AirLoopHVACOutdoorAirSystem osOutdoorAirSystem { get; set; }
        public IB_ControllerOutdoorAir ControllerOutdoorAir { get; private set; }
        //private Model osModel { get; set; }
        //private ControllerOutdoorAir osControllerOutdoorAir { get; set; }

        //include Controller inside the AirLoopHVACOutdoorAirSystem
        public IB_OutdoorAirSystem()
        {
            var model = new Model();
            this.ghostModelObject = new AirLoopHVACOutdoorAirSystem(model, new ControllerOutdoorAir(model));
            this.ControllerOutdoorAir = new IB_ControllerOutdoorAir();
        }

        public void AddController(IB_ControllerOutdoorAir ControllerOutdoorAir)
        {
            this.ControllerOutdoorAir = ControllerOutdoorAir;
        }

        public override bool AddToNode(Model model, Node node)
        {
            
            //var ctrl = (ControllerOutdoorAir)this.ControllerOutdoorAir.ToOS(ref model);

            //this.osOutdoorAirSystem = this.osOutdoorAirSystem ?? new AirLoopHVACOutdoorAirSystem(model, ctrl);
            //this.osOutdoorAirSystem.SetCustomAttributes(this.CustomAttributes);
            return ((AirLoopHVACOutdoorAirSystem)this.ToOS(model)).addToNode(node);
        }
        private AirLoopHVACOutdoorAirSystem InitMethod(Model model)
        {
            var ctrl = (ControllerOutdoorAir)this.ControllerOutdoorAir.ToOS(model);
            return new AirLoopHVACOutdoorAirSystem(model, ctrl);
        }
        public override ParentObject ToOS(Model model)
        {
            var del = new DelegateDeclaration(InitMethod);
            return (AirLoopHVACOutdoorAirSystem)this.ToOS(InitMethod, model);
        }

        public override IB_ModelObject Duplicate()
        {
            //TODO: duplicate child obj
            return this.Duplicate(() => new IB_OutdoorAirSystem());
        }
    }
}
