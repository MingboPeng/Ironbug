using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerSingleZoneHumidityMinimum : IB_SetpointManager
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerSingleZoneHumidityMinimum();

        private static SetpointManagerSingleZoneHumidityMinimum NewDefaultOpsObj(Model model)
            => new SetpointManagerSingleZoneHumidityMinimum(model);

        private string _controlZoneName { get => this.Get(string.Empty); set => this.Set(value); }
     
        public IB_SetpointManagerSingleZoneHumidityMinimum() : base(NewDefaultOpsObj(new Model()))
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
    public sealed class IB_SetpointManagerSingleZoneHumidityMinimum_FieldSet
        : IB_FieldSet<IB_SetpointManagerSingleZoneHumidityMinimum_FieldSet, SetpointManagerSingleZoneHumidityMinimum>
    {
        private IB_SetpointManagerSingleZoneHumidityMinimum_FieldSet() { }


    }
}
