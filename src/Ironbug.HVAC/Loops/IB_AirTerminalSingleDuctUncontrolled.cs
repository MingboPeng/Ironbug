using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctUncontrolled: IB_ModelObject
    {
        private static AirTerminalSingleDuctUncontrolled InitMethod(Model model) => new AirTerminalSingleDuctUncontrolled(model,model.alwaysOnDiscreteSchedule());

        public IB_AirTerminalSingleDuctUncontrolled():base(InitMethod(new Model()))
        {
            base.SetName("AirTerminal:SingleDuct:Uncontrolled");
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_AirTerminalSingleDuctUncontrolled().get();
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIB_ModelObject(() => new IB_AirTerminalSingleDuctUncontrolled());
        }
        
    }
}
