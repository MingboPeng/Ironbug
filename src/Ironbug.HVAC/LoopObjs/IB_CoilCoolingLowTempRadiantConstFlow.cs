using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingLowTempRadiantConstFlow : IB_CoilCoolingBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        private double waterHiT = 15; //59F
        private double waterLoT = 10; //50F
        private double airHiT = 25; //77F
        private double airLoT = 21; //70F


        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingLowTempRadiantConstFlow(waterHiT, waterLoT, airHiT, airLoT);

        private static CoilCoolingLowTempRadiantConstFlow NewDefaultOpsObj(Model model, double waterHiT, double waterLoT, double airHiT, double airLoT) 
            => new CoilCoolingLowTempRadiantConstFlow(model, new ScheduleRuleset(model, waterHiT), new ScheduleRuleset(model, waterLoT), new ScheduleRuleset(model, airHiT), new ScheduleRuleset(model, airLoT));

        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
            CoilCoolingLowTempRadiantConstFlow NewDefaultOpsObj(Model m)
            => new CoilCoolingLowTempRadiantConstFlow(m, 
            Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, waterHiT), 
            Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, waterLoT),
            Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, airHiT),
            Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, airLoT));
           
        }
        private IB_CoilCoolingLowTempRadiantConstFlow():base(null) { }
        public IB_CoilCoolingLowTempRadiantConstFlow(double waterHiT, double waterLoT, double airHiT, double airLoT) 
            : base(NewDefaultOpsObj(new Model(), waterHiT, waterLoT, airHiT, airLoT))
        {
            this.airHiT = airHiT;
            this.airLoT = airLoT;
            this.waterLoT = waterLoT;
            this.waterHiT = waterHiT;
        }

    }

    public sealed class IB_CoilCoolingLowTempRadiantConstFlow_FieldSet
        : IB_FieldSet<IB_CoilCoolingLowTempRadiantConstFlow_FieldSet, CoilCoolingLowTempRadiantConstFlow>
    {
        private IB_CoilCoolingLowTempRadiantConstFlow_FieldSet() { }

    }


}
