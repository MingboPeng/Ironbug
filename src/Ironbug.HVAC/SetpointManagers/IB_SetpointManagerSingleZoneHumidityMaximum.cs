using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerSingleZoneHumidityMaximum : IB_SetpointManager
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerSingleZoneHumidityMaximum();

        private static SetpointManagerSingleZoneHumidityMaximum NewDefaultOpsObj(Model model)
            => new SetpointManagerSingleZoneHumidityMaximum(model);

        private string _controlZoneName { get => this.Get(string.Empty); set => this.Set(value); } 
        public IB_SetpointManagerSingleZoneHumidityMaximum() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public void SetControlZone(string controlZoneName)
        {
            if (string.IsNullOrEmpty(controlZoneName))
                throw new ArgumentException("Invalid control zone");
            _controlZoneName = controlZoneName;
        }
        private void UpdateFromOld()
        {
            var _oldZone = this.GetChild<IB_ThermalZone>();
            if (_oldZone != null)
            {
                _controlZoneName = _oldZone.ZoneName;
                this.SetChild<IB_ThermalZone>(null);
            }
        }

        public override HVACComponent ToOS(Model model)
        {
            UpdateFromOld();

            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            // this will be executed after all loops (nodes) are saved
            Func<bool> func = () =>
            {
                var zone = model.GetThermalZone(_controlZoneName);
                if (zone == null)
                    throw new ArgumentException($"Invalid control zone ({_controlZoneName}) in {this.GetType().Name}");

                return obj.setControlZone(zone);

            };

            IB_Utility.AddDelayFunc(func);
            return obj;
        }
    }
    public sealed class IB_SetpointManagerSingleZoneHumidityMaximum_FieldSet
        : IB_FieldSet<IB_SetpointManagerSingleZoneHumidityMaximum_FieldSet, SetpointManagerSingleZoneHumidityMaximum>
    {
        private IB_SetpointManagerSingleZoneHumidityMaximum_FieldSet() { }


    }
}
