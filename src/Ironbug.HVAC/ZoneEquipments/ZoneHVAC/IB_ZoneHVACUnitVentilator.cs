using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACUnitVentilator : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACUnitVentilator();

        private static ZoneHVACUnitVentilator InitMethod(Model model) => new ZoneHVACUnitVentilator(model);

        public IB_ZoneHVACUnitVentilator() : base(InitMethod(new Model()))
        {
        }
        
        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_ZoneHVACUnitVentilator().get();
        }
    }

    public sealed class IB_ZoneHVACUnitVentilator_DataFieldSet 
        : IB_DataFieldSet<IB_ZoneHVACUnitVentilator_DataFieldSet, ZoneHVACUnitVentilator>
    {
        private IB_ZoneHVACUnitVentilator_DataFieldSet() {}

    }
}
