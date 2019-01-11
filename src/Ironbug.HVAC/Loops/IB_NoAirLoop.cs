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
        
        private IList<IB_ThermalZone> thermalZones { get; set; } = new List<IB_ThermalZone>();
        

        public IB_NoAirLoop() : base()
        {
        }
        
        public void AddThermalZones(IB_ThermalZone ThermalZones)
        {
            this.thermalZones.Add(ThermalZones);
            
        }

        public override IB_ModelObject Duplicate()
        {
            //TODO: duplicate child objects
            return this.DuplicateIBObj(IB_InitSelf);
        }
        
        protected override ModelObject InitOpsObj(Model model)
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
