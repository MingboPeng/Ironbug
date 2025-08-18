using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ElectricLoadCenterStorageConverter : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ElectricLoadCenterStorageConverter();

        private static ElectricLoadCenterStorageConverter NewDefaultOpsObj(Model model) => new ElectricLoadCenterStorageConverter(model);

        public IB_ElectricLoadCenterStorageConverter() : base(NewDefaultOpsObj)
        {
        }
       
        public ElectricLoadCenterStorageConverter ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            return obj;
        }

    }

    public sealed class IB_ElectricLoadCenterStorageConverter_FieldSet
       : IB_FieldSet<IB_ElectricLoadCenterStorageConverter_FieldSet, ElectricLoadCenterStorageConverter>
    {

        private IB_ElectricLoadCenterStorageConverter_FieldSet() { }

    }


}
