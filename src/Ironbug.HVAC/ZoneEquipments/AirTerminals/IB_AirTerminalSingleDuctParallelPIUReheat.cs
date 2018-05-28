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
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctParallelPIUReheat();
        private static AirTerminalSingleDuctParallelPIUReheat InitMethod(Model model) =>
            new AirTerminalSingleDuctParallelPIUReheat(model, model.alwaysOnDiscreteSchedule(), new FanConstantVolume(model), new CoilHeatingElectric(model));

        private IB_Child ReheatCoil => this.Children.GetChild<IB_CoilBasic>();
        private IB_Child Fan => this.Children.GetChild<IB_Fan>();
        
        
        public IB_AirTerminalSingleDuctParallelPIUReheat() : base(InitMethod(new Model()))
        {
            var reheatCoil = new IB_Child(new IB_CoilHeatingElectric(), (obj) => this.SetReheatCoil(obj as IB_CoilBasic));
            var fan = new IB_Child(new IB_FanConstantVolume(), (obj) => this.SetFan(obj as IB_Fan));
            this.Children.Add(reheatCoil);
            this.Children.Add(fan);
        }

        public void SetReheatCoil(IB_CoilBasic ReheatCoil)
        {
            this.ReheatCoil.Set(ReheatCoil);
        }
        public void SetFan(IB_Fan Fan)
        {
            this.Fan.Set(Fan);
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethodWithChildren, model).to_AirTerminalSingleDuctParallelPIUReheat().get();
            //var newOSCoil = (HVACComponent)this.ReheatCoil.To<IB_Coil>().ToOS(model);
            //var newOSFan = (HVACComponent)this.Fan.To<IB_Fan>().ToOS(model);
            //newOSObj.setReheatCoil(newOSCoil);
            //newOSObj.setFan(newOSFan);


            //Local Method
            AirTerminalSingleDuctParallelPIUReheat InitMethodWithChildren(Model md) =>
                new AirTerminalSingleDuctParallelPIUReheat(md, model.alwaysOnDiscreteSchedule(), (HVACComponent)this.Fan.To<IB_Fan>().ToOS(md), (HVACComponent)this.ReheatCoil.To<IB_CoilBasic>().ToOS(md));

        }
    }
    public sealed class IB_AirTerminalSingleDuctParallelPIUReheat_DataFieldSet 
        : IB_DataFieldSet<IB_AirTerminalSingleDuctParallelPIUReheat_DataFieldSet , AirTerminalSingleDuctParallelPIUReheat>
    {
        private IB_AirTerminalSingleDuctParallelPIUReheat_DataFieldSet() {}
    }
}
