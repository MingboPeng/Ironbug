using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_GeneratorPhotovoltaicEquivalentOneDiode : IB_Generator
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_GeneratorPhotovoltaicEquivalentOneDiode();

        private static GeneratorPhotovoltaic NewDefaultOpsObj(Model model) => GeneratorPhotovoltaic.equivalentOneDiode(model);

        public IB_GeneratorPhotovoltaicEquivalentOneDiode() : base(NewDefaultOpsObj)
        {
        }

        public override Generator ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            var perf = (obj.photovoltaicPerformance() as PhotovoltaicPerformanceEquivalentOneDiode);
            //perf.set
            //obj.set
            return obj;
        }

    }

}
