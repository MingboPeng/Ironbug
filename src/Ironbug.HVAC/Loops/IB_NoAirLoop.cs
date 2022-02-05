using Ironbug.Core;
using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ironbug.HVAC
{
    public class IB_NoAirLoop : IB_AirLoopHVAC
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_NoAirLoop();
        [DataMember]
        public List<IB_ThermalZone> ThermalZones { get; private set; } = new List<IB_ThermalZone>();

        public IB_NoAirLoop() : base()
        {
        }

        #region Serialization
        public bool ShouldSerializeThermalZones() => !this.ThermalZones.IsNullOrEmpty();
        #endregion
        public void AddThermalZones(IB_ThermalZone ThermalZones)
        {
            this.ThermalZones.Add(ThermalZones);
        }


        public override IB_ModelObject Duplicate()
        {
            var newObj = this.Duplicate(() => new IB_NoAirLoop());

            this.ThermalZones.ForEach(
                _ => newObj.AddThermalZones(_.Duplicate() as IB_ThermalZone)
                );

            return newObj;
        }

        public override ModelObject ToOS(Model model)
        {
            var tzs = this.ThermalZones;
            foreach (var item in tzs)
            {
                item.ToOS_NoAirLoop(model);
            }

            return null;
        }

        public override string ToString()
        {
            return string.Format("{0} zones in this NoAirLoop", this.ThermalZones.Count);
        }

        public override List<string> ToStrings()
        {
            return new List<string>() { this.ToString() };
        }
    }
}