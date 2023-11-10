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

        public override HVACComponent ToOS(Model model)
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
    public sealed class IB_SetpointManagerSingleZoneHumidityMaximum_FieldSet
        : IB_FieldSet<IB_SetpointManagerSingleZoneHumidityMaximum_FieldSet, SetpointManagerSingleZoneHumidityMaximum>
    {
        private IB_SetpointManagerSingleZoneHumidityMaximum_FieldSet() { }


    }
}
