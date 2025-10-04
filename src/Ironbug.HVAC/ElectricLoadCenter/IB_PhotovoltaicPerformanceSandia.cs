using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_PhotovoltaicPerformanceSandia : IB_PhotovoltaicPerformance
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_PhotovoltaicPerformanceSandia();

        private static PhotovoltaicPerformanceSandia NewDefaultOpsObj(Model model) => new PhotovoltaicPerformanceSandia(model);

        public IB_PhotovoltaicPerformanceSandia() : base(NewDefaultOpsObj)
        {
        }

        public override PhotovoltaicPerformance ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            return obj;
        }

    }

    public sealed class IB_PhotovoltaicPerformanceSandia_FieldSet
      : IB_FieldSet<IB_PhotovoltaicPerformanceSandia_FieldSet, PhotovoltaicPerformanceSandia>
    {
        private IB_PhotovoltaicPerformanceSandia_FieldSet() { }
    }
}
