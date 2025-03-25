using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SizingSystem : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SizingSystem();

        private static SizingSystem NewDefaultOpsObj(Model model) => new SizingSystem(model, new AirLoopHVAC(model));

        public IB_SizingSystem() : base(NewDefaultOpsObj)
        {
        }


        public ModelObject ToOS(Model model, AirLoopHVAC loop)
        {
            //create a new sizingPlant to target plant loop
            var old = loop.sizingSystem();
            old.SetCustomAttributes(model, this.CustomAttributes);
            return old;
        }
    }

    public sealed class IB_SizingSystem_FieldSet
        : IB_FieldSet<IB_SizingSystem_FieldSet, SizingSystem>
    {
        private IB_SizingSystem_FieldSet()
        {
        }

        public IB_Field TypeofLoadtoSizeOn { get; }
            = new IB_BasicField("TypeofLoadtoSizeOn", "typeOfLoad");

        public IB_Field DesignOutdoorAirFlowRate { get; }
            = new IB_BasicField("DesignOutdoorAirFlowRate", "OAFlowRate");

    }
}