using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_NoAirLoop : IB_AirLoopHVAC
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_NoAirLoop();
        
        private List<IB_ThermalZone> thermalZones { get; set; } = new List<IB_ThermalZone>();
        

        public IB_NoAirLoop() : base()
        {
        }
        
        public void AddThermalZones(IB_ThermalZone ThermalZones)
        {
            this.thermalZones.Add(ThermalZones);
            
        }

        public override IB_ModelObject Duplicate()
        {
            var newObj = this.DuplicateIBObj(() => new IB_NoAirLoop());

            this.thermalZones.ForEach(
                _ => newObj.AddThermalZones(_.Duplicate())
                );

            return newObj;
        }
        
        protected override ModelObject NewOpsObj(Model model)
        {
            var tzs = this.thermalZones;
            foreach (var item in tzs)
            {
                var zone = (ThermalZone)item.ToOS(model);
            }

            return null;
        }

        public override string ToString()
        {
            return string.Format("{0} zones in this NoAirLoop", this.thermalZones.Count);
        }
    }
    







}
