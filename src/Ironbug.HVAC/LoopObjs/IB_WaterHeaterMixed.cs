using System;
using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_WaterHeaterMixed : IB_HVACObject, IIB_PlantLoopObjects, IIB_DualLoopObj
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_WaterHeaterMixed();

        private static WaterHeaterMixed NewDefaultOpsObj(Model model) => new WaterHeaterMixed(model);

        private string _zone { get => this.Get(string.Empty); set => this.Set(value); }
        private IB_WaterHeaterSizing _Sizing => this.GetChild<IB_WaterHeaterSizing>();

        [JsonConstructor]
        private IB_WaterHeaterMixed(bool forDeserialization) : base(null)
        {
        }
        public IB_WaterHeaterMixed() : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(null);
        }
    
        public void SetSizing(IB_WaterHeaterSizing sizing)
        {
            this.SetChild(0, sizing);
        }

        public void SetAmbientTemperatureThermalZone(string controlZoneName)
        {
            if (string.IsNullOrEmpty(controlZoneName))
                throw new ArgumentException("Invalid control zone");
            _zone = controlZoneName;
        }

        private void UpdateFromOld()
        {
            var _oldZone = this.GetChild<IB_ThermalZone>();
            if (_oldZone != null)
            {
                _zone = _oldZone.ZoneName;
                this.SetChild<IB_ThermalZone>(null);
            }
        }


        public override HVACComponent ToOS(Model model)
        {

            UpdateFromOld();

            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);

            if (!string.IsNullOrEmpty(_zone))
            {
                // this will be executed after all loops (nodes) are saved
                Func<bool> func = () =>
                {
                    var zone = model.GetThermalZone(_zone);
                    if (zone == null)
                        throw new ArgumentException($"Invalid control zone ({_zone}) in {this.GetType().Name}");

                    return obj.setAmbientTemperatureThermalZone(zone);

                };

                IB_Utility.AddDelayFunc(func);
            }
            if (this._Sizing != null) 
                this._Sizing.ToOS(obj);
            return obj;
        }
    }

    public sealed class IB_WaterHeaterMixed_FieldSet
        : IB_FieldSet<IB_WaterHeaterMixed_FieldSet, WaterHeaterMixed>
    {
        private IB_WaterHeaterMixed_FieldSet() { }
    }
}
