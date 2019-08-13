using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingLowTempRadiantConstFlow : IB_CoilHeatingBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        private double waterHiT = 50; //122F
        private double waterLoT = 30; //86F
        private double airHiT = 20; //68F
        private double airLoT = 17; //62.6F


        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingLowTempRadiantConstFlow(waterHiT, waterLoT, airHiT, airLoT);

        private static CoilHeatingLowTempRadiantConstFlow NewDefaultOpsObj(Model model, double waterHiT, double waterLoT, double airHiT, double airLoT) 
            => new CoilHeatingLowTempRadiantConstFlow(model, new ScheduleRuleset(model, waterHiT), new ScheduleRuleset(model, waterLoT), new ScheduleRuleset(model, airHiT), new ScheduleRuleset(model, airLoT));

       
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);

            CoilHeatingLowTempRadiantConstFlow NewDefaultOpsObj(Model m)
            => new CoilHeatingLowTempRadiantConstFlow(m, 
            Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, waterHiT),
            Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, waterLoT),
            Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, airHiT),
            Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, airLoT));
      
        }



        public IB_CoilHeatingLowTempRadiantConstFlow(double waterHiT, double waterLoT, double airHiT, double airLoT) 
            : base(NewDefaultOpsObj(new Model(), waterHiT, waterLoT, airHiT, airLoT))
        {
            this.airHiT = airHiT;
            this.airLoT = airLoT;
            this.waterLoT = waterLoT;
            this.waterHiT = waterHiT;
        }

    }

    public sealed class IB_CoilHeatingLowTempRadiantConstFlow_FieldSet
        : IB_FieldSet<IB_CoilHeatingLowTempRadiantConstFlow_FieldSet, CoilHeatingLowTempRadiantConstFlow>
    {
        private IB_CoilHeatingLowTempRadiantConstFlow_FieldSet() { }

    }


}
