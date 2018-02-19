using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_AirTerminal: IB_ModelObject
    {
        private static AirTerminalSingleDuctUncontrolled InitMethod(Model model) => new AirTerminalSingleDuctUncontrolled(model,model.alwaysOnDiscreteSchedule());

        public IB_AirTerminal():base(InitMethod(new Model()))
        {
            base.SetName("AirTerminal:SingleDuct:Uncontrolled");
        }

        public override ParentObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_AirTerminalSingleDuctUncontrolled().get();
        }

        public override IB_ModelObject Duplicate()
        {
            return base.Duplicate(() => new IB_AirTerminal());
        }
        
    }
}
