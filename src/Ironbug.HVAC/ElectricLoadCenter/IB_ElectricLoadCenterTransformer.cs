using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ElectricLoadCenterTransformer : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ElectricLoadCenterTransformer();

        private static ElectricLoadCenterTransformer NewDefaultOpsObj(Model model) => new(model);

        public IB_ElectricLoadCenterTransformer() : base(NewDefaultOpsObj)
        {
        }
       
        public ElectricLoadCenterTransformer ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            return obj;
        }

    }

    public sealed class IB_ElectricLoadCenterTransformer_FieldSet
       : IB_FieldSet<IB_ElectricLoadCenterTransformer_FieldSet, ElectricLoadCenterTransformer>
    {

        private IB_ElectricLoadCenterTransformer_FieldSet() { }

    }


}
