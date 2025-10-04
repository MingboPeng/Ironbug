using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ElectricLoadCenterInverterSimple : IB_ElecInverter
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ElectricLoadCenterInverterSimple();

        private static ElectricLoadCenterInverterSimple NewDefaultOpsObj(Model model) => new ElectricLoadCenterInverterSimple(model);

        public IB_ElectricLoadCenterInverterSimple() : base(NewDefaultOpsObj)
        {
        }
       
        public override Inverter ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            return obj;
        }

    }

    public sealed class IB_ElectricLoadCenterInverterSimple_FieldSet
       : IB_FieldSet<IB_ElectricLoadCenterInverterSimple_FieldSet, ElectricLoadCenterInverterSimple>
    {

        private IB_ElectricLoadCenterInverterSimple_FieldSet() { }

    }


}
