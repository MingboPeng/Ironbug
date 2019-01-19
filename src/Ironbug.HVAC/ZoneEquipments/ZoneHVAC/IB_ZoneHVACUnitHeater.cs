using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACUnitHeater : IB_ZoneEquipment
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACUnitHeater();
        private static ZoneHVACUnitHeater InitMethod(Model model) 
            => new ZoneHVACUnitHeater(model,model.alwaysOnDiscreteSchedule(),new FanConstantVolume(model), new CoilHeatingElectric(model));

        private IB_CoilBasic HeatingCoil => this.Children.Get<IB_CoilHeatingBasic>();
        private IB_Fan Fan => this.Children.Get<IB_Fan>();
        

        public IB_ZoneHVACUnitHeater(): base(InitMethod(new Model()))
        {
            this.AddChild(new IB_CoilHeatingElectric());
            this.AddChild(new IB_FanConstantVolume());
        }

        public void SetFan(IB_Fan Fan) => this.SetChild(Fan);

        public void SetHeatingCoil(IB_CoilHeatingBasic Coil)
        {
            this.SetChild(Coil);
        }

        
        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethodWithChildren, model).to_ZoneHVACUnitHeater().get();

            //Local Method
            ZoneHVACUnitHeater InitMethodWithChildren(Model md) => 
                new ZoneHVACUnitHeater(md, md.alwaysOnDiscreteSchedule(), this.Fan.ToOS(md), this.HeatingCoil.ToOS(md));
        }
    }
    public sealed class IB_ZoneHVACUnitHeater_DataFieldSet 
        : IB_FieldSet<IB_ZoneHVACUnitHeater_DataFieldSet, ZoneHVACUnitHeater>
    {
        private IB_ZoneHVACUnitHeater_DataFieldSet() {}

    }

}
