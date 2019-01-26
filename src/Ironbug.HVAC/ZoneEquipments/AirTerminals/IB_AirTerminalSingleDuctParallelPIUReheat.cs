using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctParallelPIUReheat : IB_AirTerminal
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctParallelPIUReheat();
        private static AirTerminalSingleDuctParallelPIUReheat NewDefaultOpsObj(Model model) =>
            new AirTerminalSingleDuctParallelPIUReheat(model, model.alwaysOnDiscreteSchedule(), new FanConstantVolume(model), new CoilHeatingElectric(model));

        private IB_CoilBasic ReheatCoil => this.Children.Get<IB_CoilHeatingBasic>();
        private IB_Fan Fan => this.Children.Get<IB_Fan>();
        
        
        public IB_AirTerminalSingleDuctParallelPIUReheat() : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(new IB_CoilHeatingElectric());
            this.AddChild(new IB_FanConstantVolume());
        }

        public void SetReheatCoil(IB_CoilHeatingBasic ReheatCoil)
        {
            this.SetChild(ReheatCoil);
        }
        public void SetFan(IB_Fan Fan)
        {
            this.SetChild(Fan);
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(InitMethodWithChildren, model);
            
            AirTerminalSingleDuctParallelPIUReheat InitMethodWithChildren(Model md) =>
                new AirTerminalSingleDuctParallelPIUReheat(md, model.alwaysOnDiscreteSchedule(), this.Fan.ToOS(md), this.ReheatCoil.ToOS(md));

        }
    }
    public sealed class IB_AirTerminalSingleDuctParallelPIUReheat_DataFieldSet 
        : IB_FieldSet<IB_AirTerminalSingleDuctParallelPIUReheat_DataFieldSet , AirTerminalSingleDuctParallelPIUReheat>
    {
        private IB_AirTerminalSingleDuctParallelPIUReheat_DataFieldSet() {}
    }
}
