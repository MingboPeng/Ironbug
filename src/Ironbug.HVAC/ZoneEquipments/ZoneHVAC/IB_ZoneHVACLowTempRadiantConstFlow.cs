﻿using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACLowTempRadiantConstFlow : BaseClass.IB_ZoneEquipment
    {
        private double TubingLength { get => Get(0.0); set => Set(value, 0.0); }

        private IB_CoilHeatingLowTempRadiantConstFlow HeatingCoil => this.GetChild<IB_CoilHeatingLowTempRadiantConstFlow>();
        private IB_CoilCoolingLowTempRadiantConstFlow CoolingCoil => this.GetChild<IB_CoilCoolingLowTempRadiantConstFlow>(); 
        
        protected override Func<IB_ModelObject> IB_InitSelf 
            => () => new IB_ZoneHVACLowTempRadiantConstFlow(HeatingCoil, CoolingCoil, TubingLength);

        private static ZoneHVACLowTempRadiantConstFlow NewDefaultOpsObj(Model model, IB_CoilHeatingLowTempRadiantConstFlow HeatingCoil, IB_CoilCoolingLowTempRadiantConstFlow CoolingCoil, double TubingLength) 
            => new ZoneHVACLowTempRadiantConstFlow(model,model.alwaysOnDiscreteSchedule(), HeatingCoil.ToOS(model), CoolingCoil.ToOS(model), TubingLength);

        private IB_ZoneHVACLowTempRadiantConstFlow() : base(null) { }
        public IB_ZoneHVACLowTempRadiantConstFlow(IB_CoilHeatingLowTempRadiantConstFlow HeatingCoil, IB_CoilCoolingLowTempRadiantConstFlow CoolingCoil, double TubingLength) 
            : base((Model m) => NewDefaultOpsObj(m, HeatingCoil, CoolingCoil, TubingLength))
        {

            this.AddChild(HeatingCoil);
            this.AddChild(CoolingCoil);

            this.TubingLength = TubingLength;
        }
        
        public override HVACComponent ToOS(Model model)
        {
            var opsObj = base.OnNewOpsObj(LocalInitMethod, model);
            return opsObj;

            ZoneHVACLowTempRadiantConstFlow LocalInitMethod(Model m)
            => new ZoneHVACLowTempRadiantConstFlow(
                m, 
                m.alwaysOnDiscreteSchedule(), 
                HeatingCoil.ToOS(m), 
                CoolingCoil.ToOS(m), 
                TubingLength
                );
        }
    }

    public sealed class IB_ZoneHVACLowTempRadiantConstFlow_FieldSet
        : IB_FieldSet<IB_ZoneHVACLowTempRadiantConstFlow_FieldSet, ZoneHVACLowTempRadiantConstFlow>
    {
        internal override Type RefEpType => typeof(EPDoc.ZoneHVACLowTemperatureRadiantConstantFlow);
        private IB_ZoneHVACLowTempRadiantConstFlow_FieldSet() { }

    }
}
