using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_AirTerminalSingleDuctVAVHeatAndCoolNoReheat : IB_AirTerminal
    {
        //this is for self duplication and duplication as Puppet
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirTerminalSingleDuctVAVHeatAndCoolNoReheat();
        //this is for OpenStudio object initialization
        private static AirTerminalSingleDuctVAVHeatAndCoolNoReheat NewDefaultOpsObj(Model model) =>
            new AirTerminalSingleDuctVAVHeatAndCoolNoReheat(model);
        
        
        public IB_AirTerminalSingleDuctVAVHeatAndCoolNoReheat() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        

    }

    public sealed class IB_AirTerminalSingleDuctVAVHeatAndCoolNoReheat_FieldSet
        : IB_FieldSet<IB_AirTerminalSingleDuctVAVHeatAndCoolNoReheat_FieldSet>
    {
        private IB_AirTerminalSingleDuctVAVHeatAndCoolNoReheat_FieldSet() { }

    }

}
