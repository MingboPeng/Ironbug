using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_SolarCollectorFlatPlateWater : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SolarCollectorFlatPlateWater();

        private static SolarCollectorFlatPlateWater NewDefaultOpsObj(Model model) => new SolarCollectorFlatPlateWater(model);

        private IB_ModelObject _surface => this.GetChild<IB_ModelObject>(0);
        private IB_ModelObject _scp => this.GetChild<IB_ModelObject>(1);

        public IB_SolarCollectorFlatPlateWater() : base(NewDefaultOpsObj(new Model()))
        {
            this.AddChild(null);
            this.AddChild(null);
        }

        public void SetSurface(IB_ModelObject Surface)
        {
            this.SetChild(0, Surface);
        }

        public void SetSolarCollectorPerformance(IB_ModelObject SolarCollectorPerformance)
        {
            this.SetChild(1, SolarCollectorPerformance);
        }


        public override HVACComponent ToOS(Model model)
        {

            var opsObj = base.OnNewOpsObj(NewDefaultOpsObj, model);

            if (this._surface != null) opsObj.setSurface(this._surface.ToOS(model));
            if (this._scp != null) opsObj.setSolarCollectorPerformance(this._scp.ToOS(model));

            return opsObj;
        }
    }


    public sealed class IB_SolarCollectorFlatPlateWater_FieldSet
        : IB_FieldSet<IB_SolarCollectorFlatPlateWater_FieldSet, SolarCollectorFlatPlateWater>
    {
        private IB_SolarCollectorFlatPlateWater_FieldSet() { }
    }
}
