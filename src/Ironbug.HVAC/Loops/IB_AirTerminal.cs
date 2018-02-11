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

        public HVACComponent ToOS(ref Model model)
        {
            this.osAirTerminal = new AirTerminalSingleDuctUncontrolled(model, model.alwaysOnDiscreteSchedule());
            this.osAirTerminal.SetCustomAttributes(this.CustomAttributes);
            return this.osAirTerminal;
        }
        
    }
}
