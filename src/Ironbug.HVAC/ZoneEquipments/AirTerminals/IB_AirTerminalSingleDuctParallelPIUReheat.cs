using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctParallelPIUReheat : IB_AirTerminal
    {
        private IB_Coil ReheatCoil { get; set; } = new IB_CoilHeatingElectric();
        private IB_Fan Fan { get; set; } = new IB_FanConstantVolume();

        private static AirTerminalSingleDuctParallelPIUReheat InitMethod(Model model) =>
            new AirTerminalSingleDuctParallelPIUReheat(model,model.alwaysOnDiscreteSchedule(), new FanConstantVolume(model), new CoilHeatingElectric(model));

        public IB_AirTerminalSingleDuctParallelPIUReheat() : base(InitMethod(new Model()))
        {

        }

        public void SetReheatCoil(IB_Coil ReheatCoil)
        {
            this.ReheatCoil = ReheatCoil;
        }
        public void SetFan(IB_Fan Fan)
        {
            this.Fan = Fan;
        }
        public override IB_ModelObject Duplicate()
        {
            var newObj = (IB_AirTerminalSingleDuctParallelPIUReheat)base.DuplicateIBObj(() => new IB_AirTerminalSingleDuctParallelPIUReheat());
            var newCoil = (IB_Coil)this.ReheatCoil.Duplicate();
            var newFan = (IB_Fan)this.Fan.Duplicate();
            newObj.SetReheatCoil(newCoil);
            newObj.SetFan(newFan);

            return newObj;
        }

        public override ModelObject ToOS(Model model)
        {
            var newOSObj = base.ToOS(InitMethod, model).to_AirTerminalSingleDuctParallelPIUReheat().get();
            var newOSCoil = (HVACComponent)this.ReheatCoil.ToOS(model);
            var newOSFan = (HVACComponent)this.Fan.ToOS(model);
            newOSObj.setReheatCoil(newOSCoil);
            newOSObj.setFan(newOSFan);

            return newOSObj;
        }
    }
    public class IB_AirTerminalSingleDuctParallelPIUReheat_DataFieldSet : IB_DataFieldSet
    {
       
        protected override IddObject RefIddObject => new IdfObject(AirTerminalSingleDuctParallelPIUReheat.iddObjectType()).iddObject();

        protected override Type ParentType => typeof(AirTerminalSingleDuctParallelPIUReheat);

    }
}
