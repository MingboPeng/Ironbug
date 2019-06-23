using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_EvaporativeCoolerDirectResearchSpecial : IB_HVACObject, IIB_AirLoopObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_EvaporativeCoolerDirectResearchSpecial();

        private static EvaporativeCoolerDirectResearchSpecial NewDefaultOpsObj(Model model) => new EvaporativeCoolerDirectResearchSpecial(model, model.alwaysOnDiscreteSchedule());
        public IB_EvaporativeCoolerDirectResearchSpecial() : base(NewDefaultOpsObj(new Model()))
        {
        }
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_EvaporativeCoolerDirectResearchSpecial_FieldSet
        : IB_FieldSet<IB_EvaporativeCoolerDirectResearchSpecial_FieldSet, EvaporativeCoolerDirectResearchSpecial>
    {

        private IB_EvaporativeCoolerDirectResearchSpecial_FieldSet() { }


    }
}
