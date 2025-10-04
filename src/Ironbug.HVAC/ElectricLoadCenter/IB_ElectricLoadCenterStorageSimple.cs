using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ElectricLoadCenterStorageSimple : IB_ElecStorage
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ElectricLoadCenterStorageSimple();

        private static ElectricLoadCenterStorageSimple NewDefaultOpsObj(Model model) => new ElectricLoadCenterStorageSimple(model);

        public IB_ElectricLoadCenterStorageSimple() : base(NewDefaultOpsObj)
        {
        }
       
        public override ElectricalStorage ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            return obj;
        }

    }

    public sealed class IB_ElectricLoadCenterStorageSimple_FieldSet
       : IB_FieldSet<IB_ElectricLoadCenterStorageSimple_FieldSet, ElectricLoadCenterStorageSimple>
    {

        private IB_ElectricLoadCenterStorageSimple_FieldSet() { }

    }


}
