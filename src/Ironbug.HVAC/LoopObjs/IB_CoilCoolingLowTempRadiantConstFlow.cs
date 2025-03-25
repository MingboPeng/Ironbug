using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingLowTempRadiantConstFlow : IB_CoilCoolingBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        private double WaterHiT { get => Get<double>(15); set => Set(value, 15); } //59F
        private double WaterLoT { get => Get<double>(10); set => Set(value, 10); } //50F
        private double AirHiT { get => Get<double>(25); set => Set(value, 25); }  //77F
        private double AirLoT { get => Get<double>(21); set => Set(value, 21); } //70F


        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingLowTempRadiantConstFlow(WaterHiT, WaterLoT, AirHiT, AirLoT);

        private static CoilCoolingLowTempRadiantConstFlow NewDefaultOpsObj(Model model, double waterHiT, double waterLoT, double airHiT, double airLoT) 
            => new CoilCoolingLowTempRadiantConstFlow(model, new ScheduleRuleset(model, waterHiT), new ScheduleRuleset(model, waterLoT), new ScheduleRuleset(model, airHiT), new ScheduleRuleset(model, airLoT));

        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
            CoilCoolingLowTempRadiantConstFlow NewDefaultOpsObj(Model m)
            => new CoilCoolingLowTempRadiantConstFlow(m, 
            Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, WaterHiT), 
            Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, WaterLoT),
            Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, AirHiT),
            Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, AirLoT));
           
        }

        [JsonConstructor]
        private IB_CoilCoolingLowTempRadiantConstFlow():base(null) { }

        public IB_CoilCoolingLowTempRadiantConstFlow(double waterHiT, double waterLoT, double airHiT, double airLoT) 
            : base((Model m) => NewDefaultOpsObj(m, waterHiT, waterLoT, airHiT, airLoT))
        {
            this.AirHiT = airHiT;
            this.AirLoT = airLoT;
            this.WaterLoT = waterLoT;
            this.WaterHiT = waterHiT;
        }

    }

    public sealed class IB_CoilCoolingLowTempRadiantConstFlow_FieldSet
        : IB_FieldSet<IB_CoilCoolingLowTempRadiantConstFlow_FieldSet, CoilCoolingLowTempRadiantConstFlow>
    {
        private IB_CoilCoolingLowTempRadiantConstFlow_FieldSet() { }

    }


}
