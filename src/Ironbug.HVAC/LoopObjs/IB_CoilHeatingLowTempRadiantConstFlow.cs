using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingLowTempRadiantConstFlow : IB_CoilHeatingBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        private double WaterHiT { get => Get<double>(50); set => Set(value, 50); } //122F
        private double WaterLoT { get => Get<double>(30); set => Set(value, 30); } //86F
        private double AirHiT { get => Get<double>(20); set => Set(value, 20); }  //68F
        private double AirLoT { get => Get<double>(17); set => Set(value, 17); } //62.6F

        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingLowTempRadiantConstFlow(WaterHiT, WaterLoT, AirHiT, AirLoT);

        private static CoilHeatingLowTempRadiantConstFlow NewDefaultOpsObj(Model model, double waterHiT, double waterLoT, double airHiT, double airLoT) 
            => new CoilHeatingLowTempRadiantConstFlow(model, new ScheduleRuleset(model, waterHiT), new ScheduleRuleset(model, waterLoT), new ScheduleRuleset(model, airHiT), new ScheduleRuleset(model, airLoT));

       
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);

            CoilHeatingLowTempRadiantConstFlow NewDefaultOpsObj(Model m)
            => new CoilHeatingLowTempRadiantConstFlow(m, 
            Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, WaterHiT),
            Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, WaterLoT),
            Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, AirHiT),
            Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, AirLoT));
      
        }

        [JsonConstructor]
        private IB_CoilHeatingLowTempRadiantConstFlow() : base(null) { }

        public IB_CoilHeatingLowTempRadiantConstFlow(double waterHiT, double waterLoT, double airHiT, double airLoT) 
            : base(NewDefaultOpsObj(new Model(), waterHiT, waterLoT, airHiT, airLoT))
        {
            this.AirHiT = airHiT;
            this.AirLoT = airLoT;
            this.WaterLoT = waterLoT;
            this.WaterHiT = waterHiT;
        }

    }

    public sealed class IB_CoilHeatingLowTempRadiantConstFlow_FieldSet
        : IB_FieldSet<IB_CoilHeatingLowTempRadiantConstFlow_FieldSet, CoilHeatingLowTempRadiantConstFlow>
    {
        private IB_CoilHeatingLowTempRadiantConstFlow_FieldSet() { }

    }


}
