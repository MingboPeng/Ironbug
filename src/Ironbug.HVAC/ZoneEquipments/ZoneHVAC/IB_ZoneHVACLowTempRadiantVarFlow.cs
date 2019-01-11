using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACLowTempRadiantVarFlow : BaseClass.IB_ZoneEquipment
    {
        private IB_Child HeatingCoil => this.Children.GetChild<IB_CoilHeatingLowTempRadiantVarFlow>();
        private IB_Child CoolingCoil => this.Children.GetChild<IB_CoilCoolingLowTempRadiantVarFlow>();
        
        protected override Func<IB_ModelObject> IB_InitSelf 
            => () => new IB_ZoneHVACLowTempRadiantVarFlow(HeatingCoil.To<IB_CoilHeatingLowTempRadiantVarFlow>(), CoolingCoil.To<IB_CoilCoolingLowTempRadiantVarFlow>());

        private static ZoneHVACLowTempRadiantVarFlow InitMethod(Model model, IB_CoilHeatingLowTempRadiantVarFlow HeatingCoil, IB_CoilCoolingLowTempRadiantVarFlow CoolingCoil) 
            => new ZoneHVACLowTempRadiantVarFlow(model,model.alwaysOnDiscreteSchedule(), HeatingCoil.ToOS(model), CoolingCoil.ToOS(model));

        
        public IB_ZoneHVACLowTempRadiantVarFlow(IB_CoilHeatingLowTempRadiantVarFlow HeatingCoil, IB_CoilCoolingLowTempRadiantVarFlow CoolingCoil) 
            : base(InitMethod(new Model(), HeatingCoil, CoolingCoil))
        {
            var hC = new IB_Child(HeatingCoil, (obj) => this.HeatingCoil.Set(obj as IB_CoilHeatingLowTempRadiantVarFlow));
            var cC = new IB_Child(CoolingCoil, (obj) => this.CoolingCoil.Set(obj as IB_CoilCoolingLowTempRadiantVarFlow));

            this.Children.Add(hC);
            this.Children.Add(cC);
            
        }
        
        protected override ModelObject InitOpsObj(Model model)
        {
            var opsObj =  base.OnInitOpsObj(LocalInitMethod, model).to_ZoneHVACLowTempRadiantVarFlow().get();
            return opsObj;

            ZoneHVACLowTempRadiantVarFlow LocalInitMethod(Model m)
            => new ZoneHVACLowTempRadiantVarFlow(
                m, 
                m.alwaysOnDiscreteSchedule(), 
                HeatingCoil.To<IB_CoilHeatingLowTempRadiantVarFlow>().ToOS(m), 
                CoolingCoil.To<IB_CoilCoolingLowTempRadiantVarFlow>().ToOS(m)
                );
        }
    }

    public sealed class IB_ZoneHVACLowTempRadiantVarFlow_DataFieldSet
        : IB_FieldSet<IB_ZoneHVACLowTempRadiantVarFlow_DataFieldSet, ZoneHVACLowTempRadiantVarFlow>
    {
        private IB_ZoneHVACLowTempRadiantVarFlow_DataFieldSet() { }

    }
}
