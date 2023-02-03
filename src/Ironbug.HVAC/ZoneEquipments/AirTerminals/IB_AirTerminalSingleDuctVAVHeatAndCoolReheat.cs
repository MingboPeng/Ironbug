using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctVAVHeatAndCoolReheat : IB_AirTerminal
    {
        //this is for self duplication and duplication as Puppet
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctVAVHeatAndCoolReheat();

        //this is for OpenStudio object initialization
        private static AirTerminalSingleDuctVAVHeatAndCoolReheat NewDefaultOpsObj(Model model) =>
            new AirTerminalSingleDuctVAVHeatAndCoolReheat(model, new CoilHeatingElectric(model));

        //Associated child object
        //optional if there is no child
        private IB_CoilBasic ReheatCoil => this.GetChild<IB_CoilHeatingBasic>();

        //optional if there is no child
        public void SetReheatCoil(IB_CoilHeatingBasic ReheatCoil) => this.SetChild(ReheatCoil);

        [JsonConstructor]
        private IB_AirTerminalSingleDuctVAVHeatAndCoolReheat(bool forDeserialization) : base(null)
        {
        }

        public IB_AirTerminalSingleDuctVAVHeatAndCoolReheat() : base(NewDefaultOpsObj(new Model()))
        {
            //optional if there is no child
            //Added child with action to Children list, for later automation
            this.AddChild(new IB_CoilHeatingElectric());

        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(InitMethodWithCoil, model);

            //Local Method
            AirTerminalSingleDuctVAVHeatAndCoolReheat InitMethodWithCoil(Model md) =>
                new AirTerminalSingleDuctVAVHeatAndCoolReheat(md, this.ReheatCoil.ToOS(md));
        }
    }

    public sealed class IB_AirTerminalSingleDuctVAVHeatAndCoolReheat_FieldSet
        : IB_FieldSet<IB_AirTerminalSingleDuctVAVHeatAndCoolReheat_FieldSet>
    {
        private IB_AirTerminalSingleDuctVAVHeatAndCoolReheat_FieldSet()
        {
        }
    }
}