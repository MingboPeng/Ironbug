using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerSingleZoneCooling : IB_SetpointManager
    {
        private string _controlZoneName { get => this.Get(string.Empty); set => this.Set(value); }
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerSingleZoneCooling();

        private static SetpointManagerSingleZoneCooling NewDefaultOpsObj(Model model) 
            => new SetpointManagerSingleZoneCooling(model);

        public IB_SetpointManagerSingleZoneCooling() : base(NewDefaultOpsObj(new Model()))
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
    public sealed class IB_SetpointManagerSingleZoneCooling_FieldSet
        : IB_FieldSet<IB_SetpointManagerSingleZoneCooling_FieldSet, SetpointManagerSingleZoneCooling>
    {
        private IB_SetpointManagerSingleZoneCooling_FieldSet() { }

    }
}
