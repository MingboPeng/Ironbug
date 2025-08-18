using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_GeneratorWindTurbine : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_GeneratorWindTurbine();

        private static GeneratorWindTurbine NewDefaultOpsObj(Model model) => new(model);

        public IB_GeneratorWindTurbine() : base(NewDefaultOpsObj)
        {
        }
       
        public GeneratorWindTurbine ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            return obj;
        }

    }

    public sealed class IB_GeneratorWindTurbine_FieldSet
       : IB_FieldSet<IB_GeneratorWindTurbine_FieldSet, GeneratorWindTurbine>
    {

        private IB_GeneratorWindTurbine_FieldSet() { }

    }


}
