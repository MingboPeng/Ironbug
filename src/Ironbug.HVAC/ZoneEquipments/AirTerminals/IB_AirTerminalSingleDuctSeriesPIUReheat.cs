using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;


namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctSeriesPIUReheat : IB_AirTerminal
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctSeriesPIUReheat();

        private static AirTerminalSingleDuctSeriesPIUReheat NewDefaultOpsObj(Model model) =>
            new AirTerminalSingleDuctSeriesPIUReheat(model, new FanConstantVolume(model), new CoilHeatingElectric(model));

        private IB_CoilBasic ReheatCoil => this.GetChild<IB_CoilHeatingBasic>();
        private IB_Fan Fan => this.GetChild<IB_Fan>();

        [JsonConstructor]
        private IB_AirTerminalSingleDuctSeriesPIUReheat(bool forDeserialization) : base(null)
        {
        }

        public IB_AirTerminalSingleDuctSeriesPIUReheat() : base(NewDefaultOpsObj)
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


        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(InitMethodWithChildren, model);
            
            //Local Method
            AirTerminalSingleDuctSeriesPIUReheat InitMethodWithChildren(Model md) =>
                new AirTerminalSingleDuctSeriesPIUReheat(md, this.Fan.ToOS(md), this.ReheatCoil.ToOS(md));

        }
    }
    public sealed class IB_AirTerminalSingleDuctSeriesPIUReheat_FieldSet 
        : IB_FieldSet<IB_AirTerminalSingleDuctSeriesPIUReheat_FieldSet, AirTerminalSingleDuctSeriesPIUReheat>
    {
        private IB_AirTerminalSingleDuctSeriesPIUReheat_FieldSet() {}

    }

}
