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

        private IB_CoilBasic ReheatCoil => this.GetChild<IB_CoilHeatingBasic>();
        private IB_Fan Fan => this.GetChild<IB_Fan>();
        
        
        public IB_AirTerminalSingleDuctParallelPIUReheat() : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(new IB_CoilHeatingElectric());
            this.AddChild(new IB_FanConstantVolume());
        }

        public void SetReheatCoil(IB_CoilHeatingBasic ReheatCoil)
        {
            this.SetChild(ReheatCoil);
        }
        public void SetFan(IB_FanConstantVolume Fan)
        {
            this.SetChild(Fan);
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(InitMethodWithChildren, model);
            
            AirTerminalSingleDuctParallelPIUReheat InitMethodWithChildren(Model md) =>
                new AirTerminalSingleDuctParallelPIUReheat(md, md.alwaysOnDiscreteSchedule(), this.Fan.ToOS(md), this.ReheatCoil.ToOS(md));

        }
    }
    public sealed class IB_AirTerminalSingleDuctParallelPIUReheat_FieldSet 
        : IB_FieldSet<IB_AirTerminalSingleDuctParallelPIUReheat_FieldSet , AirTerminalSingleDuctParallelPIUReheat>
    {
        private IB_AirTerminalSingleDuctParallelPIUReheat_FieldSet() {}
    }
}
