using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_WaterHeaterMixed : IB_HVACObject, IIB_PlantLoopObjects
    {
        //protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_WaterHeaterMixed(this._Zone);
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_WaterHeaterMixed();
        private static WaterHeaterMixed NewDefaultOpsObj(Model model) => new WaterHeaterMixed(model);
        //private IB_ThermalZone _Zone => this.GetChild<IB_ThermalZone>();
        //public IB_WaterHeaterMixed(IB_ThermalZone ThermalZone) : base(NewDefaultOpsObj(new Model()))
        //{
        //    this.AddChild(ThermalZone);
        //}
        //constructor for no ambient temperature zone option
        //public IB_WaterHeaterMixed()
        //    : this(null)
        //{ }

        public IB_WaterHeaterMixed() : base(NewDefaultOpsObj(new Model())) {
        }
        //public void setAmbientTemperatureThermalZone(IB_ThermalZone Zone)
        //{
        //    this.SetChild(0, Zone);
        //}
        public override HVACComponent ToOS(Model model)
        {
            var opsObj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            //if (this._Zone != null)
            //    opsObj.setAmbientTemperatureThermalZone(this._Zone.ToOS(model) as ThermalZone);
            return opsObj;
        }
    }

    public sealed class IB_WaterHeaterMixed_FieldSet
        : IB_FieldSet<IB_WaterHeaterMixed_FieldSet, WaterHeaterMixed>
    {
        private IB_WaterHeaterMixed_FieldSet() { }
    }
}
