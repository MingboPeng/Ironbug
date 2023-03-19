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
            var zone = GetThermalZone(model, _controlZoneName);
            if (zone != null)
                obj.setControlZone(zone);

            return obj;
        }

        private static ThermalZone GetThermalZone(Model model, string name)
        {
            ThermalZone newZone = null;
            if (string.IsNullOrEmpty(name))
                return newZone;

            var optionalZone = model.getThermalZoneByName(name);
            if (optionalZone.is_initialized()) newZone = optionalZone.get();
            return newZone;

        }
    }

    public sealed class IB_AvailabilityManagerNightVentilation_FieldSet
        : IB_FieldSet<IB_AvailabilityManagerNightVentilation_FieldSet, AvailabilityManagerNightVentilation>
    {
        private IB_AvailabilityManagerNightVentilation_FieldSet() { }

    }


}
