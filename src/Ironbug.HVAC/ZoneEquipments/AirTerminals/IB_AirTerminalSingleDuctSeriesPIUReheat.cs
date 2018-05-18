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
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctSeriesPIUReheat();

        private static AirTerminalSingleDuctSeriesPIUReheat InitMethod(Model model) =>
            new AirTerminalSingleDuctSeriesPIUReheat(model, new FanConstantVolume(model), new CoilHeatingElectric(model));

        private IB_Child ReheatCoil => this.Children.GetChild<IB_Coil>();
        private IB_Child Fan => this.Children.GetChild<IB_Fan>();

        //private IB_Coil ReheatCoil { get;  set; } = new IB_CoilHeatingElectric();
        //private IB_Fan Fan { get;  set; } = new IB_FanConstantVolume();

        
        public IB_AirTerminalSingleDuctSeriesPIUReheat() : base(InitMethod(new Model()))
        {
            var reheatCoil = new IB_Child(new IB_CoilHeatingElectric(), (obj) => this.SetReheatCoil(obj as IB_Coil));
            var fan = new IB_Child(new IB_FanConstantVolume(), (obj) => this.SetFan(obj as IB_Fan));
            this.Children.Add(reheatCoil);
            this.Children.Add(fan);
        }

        public void SetReheatCoil(IB_Coil ReheatCoil)
        {
            this.ReheatCoil.Set(ReheatCoil);
        }
        public void SetFan(IB_Fan Fan)
        {
            this.Fan.Set(Fan);
        }


        //public override IB_ModelObject Duplicate()
        //{
        //    var newObj = (IB_AirTerminalSingleDuctSeriesPIUReheat)base.DuplicateIBObj(() => new IB_AirTerminalSingleDuctSeriesPIUReheat());
        //    var newCoil = (IB_Coil)this.ReheatCoil.Duplicate();
        //    var newFan = (IB_Fan)this.Fan.Duplicate();
        //    newObj.SetReheatCoil(newCoil);
        //    newObj.SetFan(newFan);

        //    return newObj;
        //}

        protected override ModelObject InitOpsObj(Model model)
        {
            var newOSObj = base.OnInitOpsObj(InitMethod, model).to_AirTerminalSingleDuctSeriesPIUReheat().get();
            var newOSCoil = (HVACComponent)this.ReheatCoil.Get<IB_Coil>().ToOS(model);
            var newOSFan = (HVACComponent)this.Fan.Get<IB_Fan>().ToOS(model);
            newOSObj.setReheatCoil(newOSCoil);
            newOSObj.setFan(newOSFan);

            return newOSObj;
        }
    }
    public sealed class IB_AirTerminalSingleDuctSeriesPIUReheat_DataFieldSet 
        : IB_DataFieldSet<IB_AirTerminalSingleDuctSeriesPIUReheat_DataFieldSet, AirTerminalSingleDuctSeriesPIUReheat>
    {
        private IB_AirTerminalSingleDuctSeriesPIUReheat_DataFieldSet() {}

    }

}
