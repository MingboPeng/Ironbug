using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingLowTempRadiantVarFlow : IB_CoilHeatingBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {

        private double AirLoT { get => Get<double>(15); set => Set(value, 15); }

        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingLowTempRadiantVarFlow(AirLoT);

        private static CoilHeatingLowTempRadiantVarFlow NewDefaultOpsObj(Model model, double airLoT) 
            => new CoilHeatingLowTempRadiantVarFlow(model, new ScheduleRuleset(model, airLoT));

        

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
            CoilHeatingLowTempRadiantVarFlow NewDefaultOpsObj(Model m)
            => new CoilHeatingLowTempRadiantVarFlow(m, Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, AirLoT));
        }

        private IB_CoilHeatingLowTempRadiantVarFlow() : base(null) { }
        public IB_CoilHeatingLowTempRadiantVarFlow(double airLoT) 
            : base(NewDefaultOpsObj(new Model(), airLoT))
        {
            this.AirLoT = airLoT;
        }

    }

    public sealed class IB_CoilHeatingLowTempRadiantVarFlow_FieldSet
        : IB_FieldSet<IB_CoilHeatingLowTempRadiantVarFlow_FieldSet, CoilHeatingLowTempRadiantVarFlow>
    {
        private IB_CoilHeatingLowTempRadiantVarFlow_FieldSet() { }

    }


}
