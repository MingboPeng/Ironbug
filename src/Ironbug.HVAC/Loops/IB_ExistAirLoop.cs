using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;

namespace Ironbug.HVAC
{
    public class IB_ExistAirLoop : IB_AirLoopHVAC, IIB_ExistingLoop
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ExistAirLoop(ExistingObj);

        private List<IB_ThermalZone> _thermalZones { get; set; } = new List<IB_ThermalZone>();
        
        public IB_ExistingObj ExistingObj { get; private set; }

        public IB_ExistAirLoop(IB_ExistingObj ExistingAirloop) : base()
        {
            this.ExistingObj = ExistingAirloop;
        }

        public void AddThermalZones(IB_ThermalZone ThermalZones)
        {
            this._thermalZones.Add(ThermalZones);
        }


        public override IB_ModelObject Duplicate()
        {
            var newObj = this.DuplicateIBObj(() => new IB_ExistAirLoop(ExistingObj));

            this._thermalZones.ForEach(
                _ => newObj.AddThermalZones(_.Duplicate() as IB_ThermalZone)
                );

            return newObj;
        }

        public override ModelObject ToOS(Model model)
        {
            var name = ExistingObj.Name;
            var optLp = model.getAirLoopHVACByName(name);
            if (optLp.isNull()) throw new ArgumentException($"Cannot find [{name}]!");

            var loop = optLp.get();
            var tzs = this._thermalZones;
            foreach (var item in tzs)
            {
                var thermalZone = item;
                var zone = (ThermalZone)item.ToOS(model);

                var airTerminal = thermalZone.AirTerminal.ToOS(model);
                if (!loop.addBranchForZone(zone, airTerminal))
                    throw new ArgumentException($"Failed to add {item.GetType()} to {this.GetType()}!");
            }

            return loop;
        }

        public override string ToString()
        {
            return string.Format("{0} zones to be added in this AirLoop", this._thermalZones.Count);
        }

        public override List<string> ToStrings()
        {
            return new List<string>() { this.ToString() };
        }
    }
}