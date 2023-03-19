using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC.AvailabilityManager
{
    public class IB_AvailabilityManagerHybridVentilation : IB_AvailabilityManager
    {
        private string _controlZoneName { get => this.Get(string.Empty); set => this.Set(value); }
        public static IB_AvailabilityManagerHybridVentilation_FieldSet FieldSet => IB_AvailabilityManagerHybridVentilation_FieldSet.Value;
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AvailabilityManagerHybridVentilation();

        private static AvailabilityManagerHybridVentilation NewDefaultOpsObj(Model model) => new AvailabilityManagerHybridVentilation(model);
        public IB_AvailabilityManagerHybridVentilation() : base(NewDefaultOpsObj(new Model()))
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
                obj.setControlledZone(zone);

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

    public sealed class IB_AvailabilityManagerHybridVentilation_FieldSet
        : IB_FieldSet<IB_AvailabilityManagerHybridVentilation_FieldSet, AvailabilityManagerHybridVentilation>
    {
        private IB_AvailabilityManagerHybridVentilation_FieldSet() { }

    }


}
