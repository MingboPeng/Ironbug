using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ThermalZone : IB_ModelObject
    {
        public IB_AirTerminal AirTerminal { get; set; }
        public List<IB_HVACComponent> ZoneEquipments { get; set; }
        

        private ThermalZone osThermalZone { get; set; }
        private SizingZone osSizingZone { get; set; }

        public IB_ThermalZone()
        {
            var model = new Model();
            this.ghostModelObject = new ThermalZone(model);
            var sizing = new SizingZone(model, this.ghostModelObject.to_ThermalZone().get());
        }
        //public void 
        public override ParentObject ToOS(Model model)
        {
            this.osThermalZone = new ThermalZone(model);
            this.osSizingZone = new SizingZone(model, this.osThermalZone);
            this.osThermalZone.SetCustomAttributes(this.CustomAttributes);
            //this.osSizingZone.SetCustomAttributes()
            return this.osThermalZone;
        }

        public override IB_ModelObject Duplicate()
        {
            //TODO: need to duplicate child objs as well
            return this.Duplicate(() => new IB_ThermalZone());
        }
    }
}
