using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ElectricLoadCenterStorageLiIonNMCBattery : IB_ElecStorage
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ElectricLoadCenterStorageLiIonNMCBattery();

        private static ElectricLoadCenterStorageLiIonNMCBattery NewDefaultOpsObj(Model model) => new ElectricLoadCenterStorageLiIonNMCBattery(model);

        public IB_ElectricLoadCenterStorageLiIonNMCBattery() : base(NewDefaultOpsObj)
        {
        }
       
        public override ElectricalStorage ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            return obj;
        }

    }

    public sealed class IB_ElectricLoadCenterStorageLiIonNMCBattery_FieldSet
       : IB_FieldSet<IB_ElectricLoadCenterStorageLiIonNMCBattery_FieldSet, ElectricLoadCenterStorageLiIonNMCBattery>
    {

        private IB_ElectricLoadCenterStorageLiIonNMCBattery_FieldSet() { }

    }


}
