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
        private static ZoneHVACUnitHeater InitMethod(Model model) 
            => new ZoneHVACUnitHeater(model,model.alwaysOnDiscreteSchedule(),new FanConstantVolume(model), new CoilHeatingElectric(model));

        private IB_Fan Fan { get; set; } = new IB_FanConstantVolume();
        private IB_Coil HeatingCoil { get; set; } = new IB_CoilHeatingElectric();

        public IB_ZoneHVACUnitHeater(): base(InitMethod(new Model()))
        {
            
        }

        public void SetFan(IB_Fan Fan)
        {
            this.Fan = Fan;
        }

        public void SetHeatingCoil(IB_Coil Coil)
        {
            //TODO: check if heating coil
            this.HeatingCoil = Coil;
        }

        public override IB_ModelObject Duplicate()
        {
            var newObj = (IB_ZoneHVACUnitHeater)base.DuplicateIBObj(() => new IB_ZoneHVACUnitHeater());
            newObj.SetFan((IB_Fan)this.Fan.Duplicate());
            newObj.SetHeatingCoil((IB_Coil)this.HeatingCoil.Duplicate());
            return newObj;

        }

        public override ModelObject ToOS(Model model)
        {
            var OsObj = base.ToOS(InitMethod, model).to_ZoneHVACUnitHeater().get();
            OsObj.setSupplyAirFan((HVACComponent)this.Fan.ToOS(model));
            OsObj.setHeatingCoil((HVACComponent)this.HeatingCoil.ToOS(model));
            return OsObj;
        }
    }
    public sealed class IB_ZoneHVACUnitHeater_DataFieldSet 
        : IB_DataFieldSet<IB_ZoneHVACUnitHeater_DataFieldSet, ZoneHVACUnitHeater>
    {
        //protected override IddObject RefIddObject => new IdfObject(ZoneHVACUnitHeater.iddObjectType()).iddObject();
        private IB_ZoneHVACUnitHeater_DataFieldSet() {}

    }

}
