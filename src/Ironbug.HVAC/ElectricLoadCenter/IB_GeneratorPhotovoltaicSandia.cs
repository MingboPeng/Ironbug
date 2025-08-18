using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_GeneratorPhotovoltaicSandia : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_GeneratorPhotovoltaicSandia();

        private static GeneratorPhotovoltaic NewDefaultOpsObj(Model model) => GeneratorPhotovoltaic.sandia(model);

        public IB_GeneratorPhotovoltaicSandia() : base(NewDefaultOpsObj)
        {
        }
       
        public GeneratorPhotovoltaic ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            var perf = (obj.photovoltaicPerformance() as PhotovoltaicPerformanceSandia);
            //perf.set
            //obj.set
            return obj;
        }

    }


}
