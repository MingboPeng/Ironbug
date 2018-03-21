using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctSeriesPIUReheat : IB_AirTerminal
    {
        public IB_Coil ReheatCoil { get; private set; } = new IB_CoilHeatingElectric();
        public IB_Fan Fan { get; private set; } = new IB_FanConstantVolume();

        private static AirTerminalSingleDuctSeriesPIUReheat InitMethod(Model model) =>
            new AirTerminalSingleDuctSeriesPIUReheat(model, new FanConstantVolume(model), new CoilHeatingElectric(model));

        public IB_AirTerminalSingleDuctSeriesPIUReheat() : base(InitMethod(new Model()))
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
            var newObj = (IB_AirTerminalSingleDuctSeriesPIUReheat)base.DuplicateIBObj(() => new IB_AirTerminalSingleDuctSeriesPIUReheat());
            var newCoil = (IB_Coil)this.ReheatCoil.Duplicate();
            var newFan = (IB_Fan)this.Fan.Duplicate();
            newObj.SetReheatCoil(newCoil);
            newObj.SetFan(newFan);

            return newObj;
        }

        public override ModelObject ToOS(Model model)
        {
            var newOSObj = base.ToOS(InitMethod, model).to_AirTerminalSingleDuctSeriesPIUReheat().get();
            var newOSCoil = (HVACComponent)this.ReheatCoil.ToOS(model);
            var newOSFan = (HVACComponent)this.Fan.ToOS(model);
            newOSObj.setReheatCoil(newOSCoil);
            newOSObj.setFan(newOSFan);

            return newOSObj;
        }
    }
}
