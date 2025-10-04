using System;
using System.Collections.Generic;
using System.Linq;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ElectricLoadCenterInverterLookUpTable : IB_ElecInverter
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ElectricLoadCenterInverterLookUpTable();

        private static ElectricLoadCenterInverterLookUpTable NewDefaultOpsObj(Model model) => new ElectricLoadCenterInverterLookUpTable(model);

        public IB_ElectricLoadCenterInverterLookUpTable() : base(NewDefaultOpsObj)
        {
        }
       
        public override Inverter ToOS(Model model)
        {
            var obj = base.OnNewOpsObj(NewDefaultOpsObj, model);
            return obj;
        }

    }

    public sealed class IB_ElectricLoadCenterInverterLookUpTable_FieldSet
       : IB_FieldSet<IB_ElectricLoadCenterInverterLookUpTable_FieldSet, ElectricLoadCenterInverterLookUpTable>
    {

        private IB_ElectricLoadCenterInverterLookUpTable_FieldSet() { }

    }


}
