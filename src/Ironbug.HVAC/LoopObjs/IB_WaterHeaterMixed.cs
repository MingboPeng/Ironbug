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
     
        private IB_ThermalZone _Zone => this.GetChild<IB_ThermalZone>();
        private IB_WaterHeaterSizing _Sizing => this.GetChild<IB_WaterHeaterSizing>();

        [JsonConstructor]
        private IB_WaterHeaterMixed(bool forDeserialization) : base(null)
        {
        }
        public IB_WaterHeaterMixed() : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(null);
            this.AddChild(null);
        }
        public void SetControllingZone(IB_ThermalZone Zone)
        {
            this.SetChild(0, Zone);
        }

        public void SetSizing(IB_WaterHeaterSizing sizing)
        {
            this.SetChild(1, sizing);
        }

        public override HVACComponent ToOS(Model model)
        {
            var opsObj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            if (this._Zone != null)
                opsObj.setAmbientTemperatureThermalZone(this._Zone.ToOS(model) as ThermalZone);
            if (this._Sizing != null) 
                this._Sizing.ToOS(opsObj);
            return opsObj;
        }
    }

    public sealed class IB_WaterHeaterMixed_FieldSet
        : IB_FieldSet<IB_WaterHeaterMixed_FieldSet, WaterHeaterMixed>
    {
        private IB_WaterHeaterMixed_FieldSet() { }
    }
}
