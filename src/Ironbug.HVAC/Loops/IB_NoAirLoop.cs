using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC
{
    public class IB_NoAirLoop : IB_AirLoopHVAC
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_NoAirLoop();

        private List<IB_ThermalZone> _thermalZones { get; set; } = new List<IB_ThermalZone>();

        public IB_NoAirLoop() : base()
        {
        }

        public void AddThermalZones(IB_ThermalZone ThermalZones)
        {
            this._thermalZones.Add(ThermalZones);
        }

        public override IB_ModelObject Duplicate()
        {
            var newObj = this.DuplicateIBObj(() => new IB_NoAirLoop());

            this._thermalZones.ForEach(
                _ => newObj.AddThermalZones(_.Duplicate())
                );

            return newObj;
        }

        public override ModelObject ToOS(Model model)
        {
            var tzs = this._thermalZones;
            foreach (var item in tzs)
            {
                var zone = (ThermalZone)item.ToOS(model);
            }

            return null;
        }

        public override string ToString()
        {
            return string.Format("{0} zones in this NoAirLoop", this._thermalZones.Count);
        }
    }
}