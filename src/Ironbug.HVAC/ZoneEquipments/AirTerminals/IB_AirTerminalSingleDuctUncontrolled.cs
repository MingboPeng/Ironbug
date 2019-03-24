using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctUncontrolled: IB_AirTerminal
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctUncontrolled();

        private static AirTerminalSingleDuctUncontrolled NewDefaultOpsObj(Model model) => new AirTerminalSingleDuctUncontrolled(model,model.alwaysOnDiscreteSchedule());

        public IB_AirTerminalSingleDuctUncontrolled():base(NewDefaultOpsObj(new Model()))
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        
        
    }

    public sealed class IB_AirTerminalSingleDuctUncontrolled_FieldSet
        : IB_FieldSet<IB_AirTerminalSingleDuctUncontrolled_FieldSet, AirTerminalSingleDuctUncontrolled>
    {
        private IB_AirTerminalSingleDuctUncontrolled_FieldSet() { }

    }

}
