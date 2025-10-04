using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_PhotovoltaicPerformanceEquivalentOneDiode : IB_PhotovoltaicPerformance
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_PhotovoltaicPerformanceEquivalentOneDiode();

        private static PhotovoltaicPerformanceEquivalentOneDiode NewDefaultOpsObj(Model model) => new PhotovoltaicPerformanceEquivalentOneDiode(model);

        public IB_PhotovoltaicPerformanceEquivalentOneDiode() : base(NewDefaultOpsObj)
        {
        }
        
        public override PhotovoltaicPerformance ToOS(Model model)
        {
            var opsObj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            return opsObj;
        }
    }

    public sealed class IB_PhotovoltaicPerformanceEquivalentOneDiode_FieldSet
        : IB_FieldSet<IB_PhotovoltaicPerformanceEquivalentOneDiode_FieldSet, PhotovoltaicPerformanceEquivalentOneDiode>
    {
        private IB_PhotovoltaicPerformanceEquivalentOneDiode_FieldSet() { }
    }
}
