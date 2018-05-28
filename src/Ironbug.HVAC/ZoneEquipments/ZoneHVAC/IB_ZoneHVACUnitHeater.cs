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

        private IB_Child HeatingCoil => this.Children.GetChild<IB_CoilBasic>();
        private IB_Child Fan => this.Children.GetChild<IB_Fan>();
        

        public IB_ZoneHVACUnitHeater(): base(InitMethod(new Model()))
        {
            var reheatCoil = new IB_Child(new IB_CoilHeatingElectric(), (obj) => this.SetHeatingCoil(obj as IB_CoilBasic));
            var fan = new IB_Child(new IB_FanConstantVolume(), (obj) => this.SetFan(obj as IB_Fan));
            this.Children.Add(reheatCoil);
            this.Children.Add(fan);
        }

        public void SetFan(IB_Fan Fan)
        {
            this.Fan.Set(Fan);
        }

        public void SetHeatingCoil(IB_CoilBasic Coil)
        {
            //TODO: check if heating coil
            this.HeatingCoil.Set(Coil);
        }

        
        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethodWithChildren, model).to_ZoneHVACUnitHeater().get();

            //Local Method
            ZoneHVACUnitHeater InitMethodWithChildren(Model md) => 
                new ZoneHVACUnitHeater(md, md.alwaysOnDiscreteSchedule(), (HVACComponent)this.Fan.To<IB_Fan>().ToOS(md), (HVACComponent)this.HeatingCoil.To<IB_CoilBasic>().ToOS(md));
        }
    }
    public sealed class IB_ZoneHVACUnitHeater_DataFieldSet 
        : IB_DataFieldSet<IB_ZoneHVACUnitHeater_DataFieldSet, ZoneHVACUnitHeater>
    {
        private IB_ZoneHVACUnitHeater_DataFieldSet() {}

    }

}
