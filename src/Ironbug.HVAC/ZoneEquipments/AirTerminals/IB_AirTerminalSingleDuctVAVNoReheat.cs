using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctVAVNoReheat : IB_AirTerminal
    {
        //this is for self duplication and duplication as Puppet
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctVAVNoReheat();
        //this is for OpenStudio object initialization
        private static AirTerminalSingleDuctVAVNoReheat NewDefaultOpsObj(Model model) =>
            new AirTerminalSingleDuctVAVNoReheat(model, model.alwaysOnDiscreteSchedule());
        
        
        public IB_AirTerminalSingleDuctVAVNoReheat() : base(NewDefaultOpsObj(new Model()))
        {
        }
        

        protected override ModelObject NewOpsObj(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model).to_AirTerminalSingleDuctVAVNoReheat().get();
        }
        

    }

    public sealed class IB_AirTerminalSingleDuctVAVNoReheat_DataFieldSet
        : IB_FieldSet<IB_AirTerminalSingleDuctVAVNoReheat_DataFieldSet, AirTerminalSingleDuctVAVNoReheat>
    {
        private IB_AirTerminalSingleDuctVAVNoReheat_DataFieldSet() { }

    }

}
