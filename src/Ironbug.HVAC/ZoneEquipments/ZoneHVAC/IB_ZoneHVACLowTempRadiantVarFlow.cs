using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACLowTempRadiantVarFlow : BaseClass.IB_ZoneEquipment
    {
        private IB_CoilHeatingLowTempRadiantVarFlow HeatingCoil => this.GetChild<IB_CoilHeatingLowTempRadiantVarFlow>();
        private IB_CoilCoolingLowTempRadiantVarFlow CoolingCoil => this.GetChild<IB_CoilCoolingLowTempRadiantVarFlow>();
        
        protected override Func<IB_ModelObject> IB_InitSelf 
            => () => new IB_ZoneHVACLowTempRadiantVarFlow(HeatingCoil, CoolingCoil);

        private static ZoneHVACLowTempRadiantVarFlow NewDefaultOpsObj(Model model, IB_CoilHeatingLowTempRadiantVarFlow HeatingCoil, IB_CoilCoolingLowTempRadiantVarFlow CoolingCoil) 
            => new ZoneHVACLowTempRadiantVarFlow(model,model.alwaysOnDiscreteSchedule(), HeatingCoil.ToOS(model), CoolingCoil.ToOS(model));

        private IB_ZoneHVACLowTempRadiantVarFlow() : base(null) { }
        public IB_ZoneHVACLowTempRadiantVarFlow(IB_CoilHeatingLowTempRadiantVarFlow HeatingCoil, IB_CoilCoolingLowTempRadiantVarFlow CoolingCoil) 
            : base(NewDefaultOpsObj(new Model(), HeatingCoil, CoolingCoil))
        {
            this.AddChild(HeatingCoil);
            this.AddChild(CoolingCoil);
        }
        
        public override HVACComponent ToOS(Model model)
        {
            var opsObj = base.OnNewOpsObj(LocalInitMethod, model);
            return opsObj;

            ZoneHVACLowTempRadiantVarFlow LocalInitMethod(Model m)
            => new ZoneHVACLowTempRadiantVarFlow(
                m, 
                m.alwaysOnDiscreteSchedule(), 
                HeatingCoil.ToOS(m), 
                CoolingCoil.ToOS(m)
                );
        }
    }

    public sealed class IB_ZoneHVACLowTempRadiantVarFlow_FieldSet
        : IB_FieldSet<IB_ZoneHVACLowTempRadiantVarFlow_FieldSet, ZoneHVACLowTempRadiantVarFlow>
    {
        internal override Type RefEpType => typeof(EPDoc.ZoneHVACLowTemperatureRadiantVariableFlow);
        private IB_ZoneHVACLowTempRadiantVarFlow_FieldSet() { }

    }
}
