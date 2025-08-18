using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_GeneratorMicroTurbine : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_GeneratorMicroTurbine();

        private static GeneratorMicroTurbine NewDefaultOpsObj(Model model) => new(model);

        public IB_GeneratorMicroTurbine() : base(NewDefaultOpsObj)
        {
        }
       
        public GeneratorMicroTurbine ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            return obj;
        }

    }

    public sealed class IB_GeneratorMicroTurbine_FieldSet
       : IB_FieldSet<IB_GeneratorMicroTurbine_FieldSet, GeneratorMicroTurbine>
    {

        private IB_GeneratorMicroTurbine_FieldSet() { }

    }


}
