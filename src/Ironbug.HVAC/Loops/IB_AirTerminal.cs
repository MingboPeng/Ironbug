using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_AirTerminal: IB_ModelObject
    {
        private HVACComponent osAirTerminal { get; set; }

        public IB_AirTerminal()
        {
            var model = new Model();
            this.ghostModelObject = new AirTerminalSingleDuctUncontrolled(model, model.alwaysOnDiscreteSchedule());
        }

        public override ParentObject ToOS(Model model)
        {
            this.osAirTerminal = new AirTerminalSingleDuctUncontrolled(model, model.alwaysOnDiscreteSchedule());
            this.osAirTerminal.SetCustomAttributes(this.CustomAttributes);
            return this.osAirTerminal;
        }

        //private static ControllerOutdoorAir InitMethod(ref Model model) => new ControllerOutdoorAir(model);
        //public override ParentObject ToOS(ref Model model)
        //{
        //    return (ControllerOutdoorAir)this.ToOS(InitMethod, ref model);
        //}
    }
}
