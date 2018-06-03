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

        private static AirTerminalSingleDuctUncontrolled InitMethod(Model model) => new AirTerminalSingleDuctUncontrolled(model,model.alwaysOnDiscreteSchedule());

        public IB_AirTerminalSingleDuctUncontrolled():base(InitMethod(new Model()))
        {
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_AirTerminalSingleDuctUncontrolled().get();
        }
        
        
    }

    public sealed class IB_AirTerminalSingleDuctUncontrolled_DataFieldSet
        : IB_FieldSet<IB_AirTerminalSingleDuctUncontrolled_DataFieldSet, AirTerminalSingleDuctUncontrolled>
    {
        private IB_AirTerminalSingleDuctUncontrolled_DataFieldSet() { }

    }

}
