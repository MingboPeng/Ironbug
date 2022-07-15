using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_PhotovoltaicPerformanceSimple : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_PhotovoltaicPerformanceSimple();

        private static PhotovoltaicPerformanceSimple NewDefaultOpsObj(Model model) => new PhotovoltaicPerformanceSimple(model);
        public IB_PhotovoltaicPerformanceSimple() : base(NewDefaultOpsObj(new Model()))
        {
        }
        public void setAmbientTemperatureThermalZone()
        {
        }
        public override IB_ModelObject ToOS(Model model)
        {
            return opsObj = base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_PhotovoltaicPerformanceSimple_FieldSet
        : IB_FieldSet<IB_PhotovoltaicPerformanceSimple_FieldSet, PhotovoltaicPerformanceSimple>
    {
        private IB_PhotovoltaicPerformanceSimple_FieldSet() { }
    }
}
