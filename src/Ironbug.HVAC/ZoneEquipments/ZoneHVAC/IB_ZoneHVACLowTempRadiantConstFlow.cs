using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACLowTempRadiantConstFlow : BaseClass.IB_ZoneEquipment
    {
        private IB_Child HeatingCoil => this.Children.GetChild<IB_CoilHeatingLowTempRadiantConstFlow>();
        private IB_Child CoolingCoil => this.Children.GetChild<IB_CoilCoolingLowTempRadiantConstFlow>();
        //private IB_CoilHeatingLowTempRadiantConstFlow HeatingCoil;
        //private IB_CoilCoolingLowTempRadiantConstFlow CoolingCoil;
        private double TubingLength;


        protected override Func<IB_ModelObject> IB_InitSelf 
            => () => new IB_ZoneHVACLowTempRadiantConstFlow(HeatingCoil.To<IB_CoilHeatingLowTempRadiantConstFlow>(), CoolingCoil.To<IB_CoilCoolingLowTempRadiantConstFlow>(), TubingLength);

        private static ZoneHVACLowTempRadiantConstFlow InitMethod(Model model, IB_CoilHeatingLowTempRadiantConstFlow HeatingCoil, IB_CoilCoolingLowTempRadiantConstFlow CoolingCoil, double TubingLength) 
            => new ZoneHVACLowTempRadiantConstFlow(model,model.alwaysOnDiscreteSchedule(), HeatingCoil.ToOS(model), CoolingCoil.ToOS(model), TubingLength);

        
        public IB_ZoneHVACLowTempRadiantConstFlow(IB_CoilHeatingLowTempRadiantConstFlow HeatingCoil, IB_CoilCoolingLowTempRadiantConstFlow CoolingCoil, double TubingLength) 
            : base(InitMethod(new Model(), HeatingCoil, CoolingCoil, TubingLength))
        {
            var hC = new IB_Child(HeatingCoil, (obj) => this.HeatingCoil.Set(obj as IB_CoilHeatingLowTempRadiantConstFlow));
            var cC = new IB_Child(CoolingCoil, (obj) => this.CoolingCoil.Set(obj as IB_CoilCoolingLowTempRadiantConstFlow));

            this.Children.Add(hC);
            this.Children.Add(cC);
            //this.HeatingCoil = HeatingCoil;
            //this.CoolingCoil = CoolingCoil;
            this.TubingLength = TubingLength;
        }
        
        protected override ModelObject InitOpsObj(Model model)
        {
            var opsObj =  base.OnInitOpsObj(LocalInitMethod, model).to_ZoneHVACLowTempRadiantConstFlow().get();
            return opsObj;

            ZoneHVACLowTempRadiantConstFlow LocalInitMethod(Model m)
            => new ZoneHVACLowTempRadiantConstFlow(
                m, 
                m.alwaysOnDiscreteSchedule(), 
                HeatingCoil.To<IB_CoilHeatingLowTempRadiantConstFlow>().ToOS(m), 
                CoolingCoil.To<IB_CoilCoolingLowTempRadiantConstFlow>().ToOS(m), 
                TubingLength
                );
        }
    }

    public sealed class IB_ZoneHVACLowTempRadiantConstFlow_DataFieldSet
        : IB_FieldSet<IB_ZoneHVACLowTempRadiantConstFlow_DataFieldSet, ZoneHVACLowTempRadiantConstFlow>
    {
        private IB_ZoneHVACLowTempRadiantConstFlow_DataFieldSet() { }

    }
}
