using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;


namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctSeriesPIUReheat : IB_AirTerminal
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctSeriesPIUReheat();

        private static AirTerminalSingleDuctSeriesPIUReheat NewDefaultOpsObj(Model model) =>
            new AirTerminalSingleDuctSeriesPIUReheat(model, new FanConstantVolume(model), new CoilHeatingElectric(model));

        private IB_CoilBasic ReheatCoil => this.Children.Get<IB_CoilHeatingBasic>();
        private IB_Fan Fan => this.Children.Get<IB_Fan>();
        
        public IB_AirTerminalSingleDuctSeriesPIUReheat() : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(new IB_CoilHeatingElectric());
            this.AddChild(new IB_FanConstantVolume());
        }

        public void SetReheatCoil(IB_CoilHeatingBasic heatingCoil)
        {
            this.SetChild(heatingCoil);
        }
        public void SetFan(IB_Fan fan)
        {
            if (!(fan is IB_FanConstantVolume))
            {
                throw new Exception("I think SeriesPIUReheat box only accepts the FanConstantVolume. Please let me know if I was wrong!");
            }
            this.SetChild(fan);
        }
        

        protected override ModelObject NewOpsObj(Model model)
        {
            return base.OnNewOpsObj(InitMethodWithChildren, model).to_AirTerminalSingleDuctSeriesPIUReheat().get();
            
            //Local Method
            AirTerminalSingleDuctSeriesPIUReheat InitMethodWithChildren(Model md) =>
                new AirTerminalSingleDuctSeriesPIUReheat(md, this.Fan.ToOS(md), this.ReheatCoil.ToOS(md));

        }
    }
    public sealed class IB_AirTerminalSingleDuctSeriesPIUReheat_DataFieldSet 
        : IB_FieldSet<IB_AirTerminalSingleDuctSeriesPIUReheat_DataFieldSet, AirTerminalSingleDuctSeriesPIUReheat>
    {
        private IB_AirTerminalSingleDuctSeriesPIUReheat_DataFieldSet() {}

    }

}
