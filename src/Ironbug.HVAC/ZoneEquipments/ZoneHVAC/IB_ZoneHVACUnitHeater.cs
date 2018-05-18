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

        private IB_Child HeatingCoil => this.Children.GetChild<IB_Coil>();
        private IB_Child Fan => this.Children.GetChild<IB_Fan>();

        //private IB_Fan Fan { get; set; } = new IB_FanConstantVolume();
        //private IB_Coil HeatingCoil { get; set; } = new IB_CoilHeatingElectric();

        

        public IB_ZoneHVACUnitHeater(): base(InitMethod(new Model()))
        {
            var reheatCoil = new IB_Child(new IB_CoilHeatingElectric(), (obj) => this.SetHeatingCoil(obj as IB_Coil));
            var fan = new IB_Child(new IB_FanConstantVolume(), (obj) => this.SetFan(obj as IB_Fan));
            this.Children.Add(reheatCoil);
            this.Children.Add(fan);
        }

        public void SetFan(IB_Fan Fan)
        {
            this.Fan.Set(Fan);
        }

        public void SetHeatingCoil(IB_Coil Coil)
        {
            //TODO: check if heating coil
            this.HeatingCoil.Set(Coil);
        }

        //public override IB_ModelObject Duplicate()
        //{
        //    var newObj = (IB_ZoneHVACUnitHeater)base.DuplicateIBObj(() => new IB_ZoneHVACUnitHeater());
        //    newObj.SetFan((IB_Fan)this.Fan.Duplicate());
        //    newObj.SetHeatingCoil((IB_Coil)this.HeatingCoil.Duplicate());
        //    return newObj;

        //}

        protected override ModelObject InitOpsObj(Model model)
        {
            var OsObj = base.OnInitOpsObj(InitMethod, model).to_ZoneHVACUnitHeater().get();
            OsObj.setSupplyAirFan((HVACComponent)this.Fan.Get<IB_Fan>().ToOS(model));
            OsObj.setHeatingCoil((HVACComponent)this.HeatingCoil.Get<IB_Coil>().ToOS(model));
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
