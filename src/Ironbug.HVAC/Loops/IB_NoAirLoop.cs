using Ironbug.Core;
using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Ironbug.HVAC
{
    public class IB_NoAirLoop : IB_AirLoopHVAC, IEquatable<IB_NoAirLoop>
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
            var newObj = new IB_NoAirLoop();
            newObj.ThermalZones = this.ThermalZones.Select(_=> _.Duplicate() as IB_ThermalZone).ToList();
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

        public override List<IB_ThermalZone> GetThermalZones()
        {
            return this.ThermalZones;
        }
        public override string ToString()
        {
            return string.Format("{0} zones in this NoAirLoop", this.ThermalZones.Count);
        }

        public override List<string> ToStrings()
        {
            return new List<string>() { this.ToString() };
        }

        public override bool Equals(object obj) => this.Equals(obj as IB_NoAirLoop);
        public bool Equals(IB_NoAirLoop other)
        {
            if (other is null)
                return false;
            return this.ThermalZones.SequenceEqual(other.ThermalZones);
        }
    }
}