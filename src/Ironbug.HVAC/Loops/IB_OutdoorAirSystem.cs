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
        private AirLoopHVACOutdoorAirSystem osOutdoorAirSystem { get; set; }
        public IB_ControllerOutdoorAir ControllerOutdoorAir { get; set; }
        private Model osModel { get; set; }
        //private ControllerOutdoorAir osControllerOutdoorAir { get; set; }

        //include Controller inside the AirLoopHVACOutdoorAirSystem
        public IB_OutdoorAirSystem()
        {
            var model = new Model();
            this.ghostModelObject = new AirLoopHVACOutdoorAirSystem(model, new ControllerOutdoorAir(model));
            this.ControllerOutdoorAir = new IB_ControllerOutdoorAir();
        }


        public override bool AddToNode(ref Model model, Node node)
        {
            //this.osModel = model;
            //var osControllerOutdoorAir = new ControllerOutdoorAir(osModel);
            //osControllerOutdoorAir.SetCustomAttributes(this.ControllerOutdoorAir.CustomAttributes);
            var ctrl = (ControllerOutdoorAir)this.ControllerOutdoorAir.ToOS(ref model);

            this.osOutdoorAirSystem = this.osOutdoorAirSystem ?? new AirLoopHVACOutdoorAirSystem(osModel, ctrl);
            this.osOutdoorAirSystem.SetCustomAttributes(this.CustomAttributes);
            return this.osOutdoorAirSystem.addToNode(node);
        }
        //private static ControllerOutdoorAir InitMethod(ref Model model) => new ControllerOutdoorAir(model);

        private static AirLoopHVACOutdoorAirSystem InitMethod(ref Model model)
        {
            return new AirLoopHVACOutdoorAirSystem(model, new ControllerOutdoorAir(model));
        }
        public override ParentObject ToOS(ref Model model)
        {
            return (AirLoopHVACOutdoorAirSystem)this.ToOS(InitMethod, ref model);
        }
    }
}
