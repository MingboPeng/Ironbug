using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctConstantVolumeReheat : IB_AirTerminal
    {
        //this is for self duplication and duplication as Puppet
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctConstantVolumeReheat();

        //this is for OpenStudio object initialization
        private static AirTerminalSingleDuctConstantVolumeReheat NewDefaultOpsObj(Model model) =>
            new AirTerminalSingleDuctConstantVolumeReheat(model, model.alwaysOnDiscreteSchedule(), new CoilHeatingElectric(model));

        //Associated child object
        //optional if there is no child
        private IB_CoilBasic ReheatCoil => this.GetChild<IB_CoilHeatingBasic>();

        //optional if there is no child
        public void SetReheatCoil(IB_CoilHeatingBasic ReheatCoil) => this.SetChild(ReheatCoil);

        [JsonConstructor]
        private IB_AirTerminalSingleDuctConstantVolumeReheat(bool forDeserialization) : base(null) { }

        public IB_AirTerminalSingleDuctConstantVolumeReheat() : base(NewDefaultOpsObj)
        {
            //optional if there is no child
            //Added child with action to Children list, for later automation
            this.AddChild(new IB_CoilHeatingElectric());
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(InitMethodWithCoil, model);

            //Local Method
            AirTerminalSingleDuctConstantVolumeReheat InitMethodWithCoil(Model md) =>
                new AirTerminalSingleDuctConstantVolumeReheat(md, md.alwaysOnDiscreteSchedule(), this.ReheatCoil.ToOS(md));
        }
    }

    public sealed class IB_AirTerminalSingleDuctConstantVolumeReheat_FieldSet
        : IB_FieldSet<IB_AirTerminalSingleDuctConstantVolumeReheat_FieldSet>
    {
        private IB_AirTerminalSingleDuctConstantVolumeReheat_FieldSet()
        {
        }
    }
}