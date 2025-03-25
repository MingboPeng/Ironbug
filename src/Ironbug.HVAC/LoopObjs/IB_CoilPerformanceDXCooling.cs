using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilPerformanceDXCooling : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilPerformanceDXCooling();

        private static CoilPerformanceDXCooling NewDefaultOpsObj(Model model) => new CoilPerformanceDXCooling(model);

        public IB_CoilPerformanceDXCooling() : base(NewDefaultOpsObj)
        {
            
        }

        public CoilPerformanceDXCooling ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }

    }

    public sealed class IB_CoilPerformanceDXCooling_FieldSet
        : IB_FieldSet<IB_CoilPerformanceDXCooling_FieldSet, CoilPerformanceDXCooling>
    {
        private IB_CoilPerformanceDXCooling_FieldSet() { }

    }
}
