using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_HumidifierSteamElectric : IB_HVACObject, IIB_AirLoopObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_HumidifierSteamElectric();
        private static HumidifierSteamElectric NewDefaultOpsObj(Model model) => new HumidifierSteamElectric(model);

        public IB_HumidifierSteamElectric() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_HumidifierSteamElectric_FieldSet
     : IB_FieldSet<IB_HumidifierSteamElectric_FieldSet, HumidifierSteamElectric>
    {
        private IB_HumidifierSteamElectric_FieldSet()
        {
        }

    }
}
