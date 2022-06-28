using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_SolarCollectorFlatPlateWater : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SolarCollectorFlatPlateWater();

        private static SolarCollectorFlatPlateWater NewDefaultOpsObj(Model model) => new SolarCollectorFlatPlateWater(model);
        private IB_PlanarSurface _surface => this.GetChild<IB_PlanarSurface>(0);
        public IB_SolarCollectorFlatPlateWater() : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(null);
        }
        public void SetSurface(IB_PlanarSurface surface)
        {
            this.SetChild(0, surface);
        }
        public override HVACComponent ToOS(Model model)
        {

            var opsObj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            //changed input from string to surface - Question: how does one convert HB surface to OS surface
            if (this._surface != null) opsObj.setSurface(this._surface);

            return opsObj;
        }
    }

    public sealed class IB_SolarCollectorFlatPlateWater_FieldSet
        : IB_FieldSet<IB_SolarCollectorFlatPlateWater_FieldSet, SolarCollectorFlatPlateWater>
    {
        private IB_SolarCollectorFlatPlateWater_FieldSet() { }
    }
}
