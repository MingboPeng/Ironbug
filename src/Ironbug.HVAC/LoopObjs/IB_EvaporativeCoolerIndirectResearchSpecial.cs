using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_EvaporativeCoolerIndirectResearchSpecial : IB_HVACObject, IIB_AirLoopObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_EvaporativeCoolerIndirectResearchSpecial();

        private static EvaporativeCoolerIndirectResearchSpecial NewDefaultOpsObj(Model model) => new EvaporativeCoolerIndirectResearchSpecial(model);
        public IB_EvaporativeCoolerIndirectResearchSpecial() : base(NewDefaultOpsObj)
        {
        }
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_EvaporativeCoolerIndirectResearchSpecial_FieldSet
        : IB_FieldSet<IB_EvaporativeCoolerIndirectResearchSpecial_FieldSet, EvaporativeCoolerIndirectResearchSpecial>
    {

        private IB_EvaporativeCoolerIndirectResearchSpecial_FieldSet() { }


    }
}
