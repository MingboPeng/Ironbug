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
        
        
        private SizingZone osSizingZone { get; set; }
        private static ThermalZone InitMethod(Model model) => new ThermalZone(model);
        public IB_ThermalZone():base(InitMethod(new Model()))
        {
            //TODO: this should be the same as HBZone name
            base.SetName("ThermalZone");
            var model = base.GhostOSObject.model();
            
            //TODO: this doesn't look clear to me, try to find a better way
            var sizing = new SizingZone(model, this.GhostOSObject.to_ThermalZone().get());

        }
        
        public override ParentObject ToOS(Model model)
        {
            var zone = (ThermalZone)base.ToOS(InitMethod, model);

            this.osSizingZone = new SizingZone(model, zone);
            //this.osSizingZone.SetCustomAttributes()
            return zone;
        }

        public override IB_ModelObject Duplicate()
        {
            //TODO: need to duplicate child objs as well
            var newObj = (IB_ThermalZone)this.Duplicate(() => new IB_ThermalZone());

            foreach (var item in this.ZoneEquipments)
            {
                var newItem = (IB_HVACComponent)item.Duplicate();
                newObj.ZoneEquipments.Add(newItem);
            }

            
            return newObj;
        }
    }
}
