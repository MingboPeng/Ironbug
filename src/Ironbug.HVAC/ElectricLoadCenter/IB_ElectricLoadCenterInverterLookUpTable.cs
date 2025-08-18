using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ElectricLoadCenterInverterPVWatts : IB_ModelObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ElectricLoadCenterInverterPVWatts();

        private static ElectricLoadCenterInverterPVWatts NewDefaultOpsObj(Model model) => new ElectricLoadCenterInverterPVWatts(model);

        public IB_ElectricLoadCenterInverterPVWatts() : base(NewDefaultOpsObj)
        {
        }
       
        public ElectricLoadCenterInverterPVWatts ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            return obj;
        }

    }

    public sealed class IB_ElectricLoadCenterInverterPVWatts_FieldSet
       : IB_FieldSet<IB_ElectricLoadCenterInverterPVWatts_FieldSet, ElectricLoadCenterInverterPVWatts>
    {

        private IB_ElectricLoadCenterInverterPVWatts_FieldSet() { }

    }


}
