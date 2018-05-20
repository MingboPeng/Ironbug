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
        

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethodWithChildren, model).to_AirTerminalSingleDuctSeriesPIUReheat().get();
            
            //Local Method
            AirTerminalSingleDuctSeriesPIUReheat InitMethodWithChildren(Model md) =>
                new AirTerminalSingleDuctSeriesPIUReheat(md, (HVACComponent)this.Fan.To<IB_Fan>().ToOS(md), (HVACComponent)this.ReheatCoil.To<IB_Coil>().ToOS(md));

        }
    }
    public sealed class IB_AirTerminalSingleDuctSeriesPIUReheat_DataFieldSet 
        : IB_DataFieldSet<IB_AirTerminalSingleDuctSeriesPIUReheat_DataFieldSet, AirTerminalSingleDuctSeriesPIUReheat>
    {
        private IB_AirTerminalSingleDuctSeriesPIUReheat_DataFieldSet() {}

    }

}
