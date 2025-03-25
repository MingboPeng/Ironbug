using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctVAVReheat : IB_AirTerminal
    {
        //this is for self duplication and duplication as Puppet
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctVAVReheat();

        //this is for OpenStudio object initialization
        private static AirTerminalSingleDuctVAVReheat NewDefaultOpsObj(Model model) =>
            new AirTerminalSingleDuctVAVReheat(model, model.alwaysOnDiscreteSchedule(), new CoilHeatingWater(model));

        //Associated child object
        //optional if there is no child
        private IB_CoilBasic ReheatCoil => this.GetChild<IB_CoilHeatingBasic>();

        //optional if there is no child
        public void SetReheatCoil(IB_CoilHeatingBasic ReheatCoil) => this.SetChild(ReheatCoil);

        [JsonConstructor]
        private IB_AirTerminalSingleDuctVAVReheat(bool forDeserialization) : base(null)
        {
        }

        public IB_AirTerminalSingleDuctVAVReheat() : base(NewDefaultOpsObj)
        {
            //optional if there is no child
            //Added child with action to Children list, for later automation
            this.AddChild(new IB_CoilHeatingWater());
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(InitMethodWithCoil, model);

            //Local Method
            AirTerminalSingleDuctVAVReheat InitMethodWithCoil(Model md) =>
                new AirTerminalSingleDuctVAVReheat(md, md.alwaysOnDiscreteSchedule(), this.ReheatCoil.ToOS(md));
        }
    }

    public sealed class IB_AirTerminalSingleDuctVAVReheat_FieldSet
        : IB_FieldSet<IB_AirTerminalSingleDuctVAVReheat_FieldSet, AirTerminalSingleDuctVAVReheat>
    {
        private IB_AirTerminalSingleDuctVAVReheat_FieldSet()
        {
        }
    }
}