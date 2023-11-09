using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.AvailabilityManager
{
    public class IB_AvailabilityManagerNightVentilation : IB_AvailabilityManager
    {
        private string _controlZoneName { get => this.Get(string.Empty); set => this.Set(value); }
        public static IB_AvailabilityManagerNightVentilation_FieldSet FieldSet => IB_AvailabilityManagerNightVentilation_FieldSet.Value;
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AvailabilityManagerNightVentilation();

        private static AvailabilityManagerNightVentilation NewDefaultOpsObj(Model model) => new AvailabilityManagerNightVentilation(model);
        public IB_AvailabilityManagerNightVentilation() : base(NewDefaultOpsObj(new Model()))
        {
        } 

        public void SetControlZone(string controlZoneName)
        {
            if (string.IsNullOrEmpty(controlZoneName))
                return;
            _controlZoneName = controlZoneName;
        }

        public override OpenStudio.AvailabilityManager ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            // this will be executed after all loops (nodes) are saved
            Func<bool> func = () =>
            {
                var zone = model.GetThermalZone(_controlZoneName);
                if (zone == null)
                    return false;

                return obj.setControlZone(zone);

            };

            IB_Utility.AddDelayFunc(func);

            return obj;
        }

    }

    public sealed class IB_AvailabilityManagerNightVentilation_FieldSet
        : IB_FieldSet<IB_AvailabilityManagerNightVentilation_FieldSet, AvailabilityManagerNightVentilation>
    {
        private IB_AvailabilityManagerNightVentilation_FieldSet() { }

    }


}
