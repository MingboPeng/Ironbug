using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_GeneratorPhotovoltaicSimple : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_GeneratorPhotovoltaicSimple();

        private static GeneratorPhotovoltaic NewDefaultOpsObj(Model model) => GeneratorPhotovoltaic.simple(model);

        public IB_GeneratorPhotovoltaicSimple() : base(NewDefaultOpsObj)
        {
        }
       
        public GeneratorPhotovoltaic ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            var perf = (obj.photovoltaicPerformance() as PhotovoltaicPerformanceSimple);
            //perf.set
            //obj.set
            return obj;
        }

    }

    public sealed class IB_GeneratorPhotovoltaic_FieldSet
       : IB_FieldSet<IB_GeneratorPhotovoltaic_FieldSet, GeneratorPhotovoltaic>
    {

        private IB_GeneratorPhotovoltaic_FieldSet() { }

    }


}
