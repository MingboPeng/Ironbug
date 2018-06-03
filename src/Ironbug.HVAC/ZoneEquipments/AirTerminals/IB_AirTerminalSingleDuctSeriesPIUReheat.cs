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

        private IB_Child ReheatCoil => this.Children.GetChild<IB_CoilBasic>();
        private IB_Child Fan => this.Children.GetChild<IB_Fan>();
        
        public IB_AirTerminalSingleDuctSeriesPIUReheat() : base(InitMethod(new Model()))
        {
            var reheatCoil = new IB_Child(new IB_CoilHeatingElectric(), (obj) => this.SetReheatCoil(obj as IB_CoilBasic));
            var fan = new IB_Child(new IB_FanConstantVolume(), (obj) => this.SetFan(obj as IB_Fan));
            this.Children.Add(reheatCoil);
            this.Children.Add(fan);
        }

        public void SetReheatCoil(IB_CoilBasic heatingCoil)
        {
            this.ReheatCoil.Set(heatingCoil);
        }
        public void SetFan(IB_Fan fan)
        {
            if (!(fan is IB_FanConstantVolume))
            {
                throw new Exception("I think SeriesPIUReheat box only accepts the FanConstantVolume. Please let me know if I was wrong!");
            }
            this.Fan.Set(fan);
        }
        

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethodWithChildren, model).to_AirTerminalSingleDuctSeriesPIUReheat().get();
            
            //Local Method
            AirTerminalSingleDuctSeriesPIUReheat InitMethodWithChildren(Model md) =>
                new AirTerminalSingleDuctSeriesPIUReheat(md, (HVACComponent)this.Fan.To<IB_Fan>().ToOS(md), (HVACComponent)this.ReheatCoil.To<IB_CoilBasic>().ToOS(md));

        }
    }
    public sealed class IB_AirTerminalSingleDuctSeriesPIUReheat_DataFieldSet 
        : IB_FieldSet<IB_AirTerminalSingleDuctSeriesPIUReheat_DataFieldSet, AirTerminalSingleDuctSeriesPIUReheat>
    {
        private IB_AirTerminalSingleDuctSeriesPIUReheat_DataFieldSet() {}

    }

}
