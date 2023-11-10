using System;
using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_WaterHeaterMixed : IB_HVACObject, IIB_PlantLoopObjects
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

        public void setAmbientTemperatureThermalZone(string controlZoneName)
        {
            if (string.IsNullOrEmpty(controlZoneName))
                throw new ArgumentException("Invalid control zone");
            _zone = controlZoneName;
        }


        public override HVACComponent ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);

            if (!string.IsNullOrEmpty(_zone))
            {
                // this will be executed after all loops (nodes) are saved
                Func<bool> func = () =>
                {
                    var zone = model.GetThermalZone(_zone);
                    if (zone == null)
                        return false;

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
