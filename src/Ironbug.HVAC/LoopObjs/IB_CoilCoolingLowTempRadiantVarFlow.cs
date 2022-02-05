using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;


namespace Ironbug.HVAC
{
    public class IB_CoilCoolingLowTempRadiantVarFlow : IB_CoilCoolingBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        private double airHiT = 50; 
        
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingLowTempRadiantVarFlow( airHiT);

        private static CoilCoolingLowTempRadiantVarFlow NewDefaultOpsObj(Model model, double airHiT) 
            => new CoilCoolingLowTempRadiantVarFlow(model, new ScheduleRuleset(model, airHiT));

      

        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);

            
            CoilCoolingLowTempRadiantVarFlow NewDefaultOpsObj(Model m) 
                => new CoilCoolingLowTempRadiantVarFlow(m, Schedules.IB_ScheduleRuleset.GetOrNewConstantSchedule(m, airHiT));
          

        }
        private IB_CoilCoolingLowTempRadiantVarFlow() : base(null) { }
        public IB_CoilCoolingLowTempRadiantVarFlow( double airHiT) 
            : base(NewDefaultOpsObj(new Model(), airHiT))
        {
            this.airHiT = airHiT;
        }

    }

    public sealed class IB_CoilCoolingLowTempRadiantVarFlow_FieldSet
        : IB_FieldSet<IB_CoilCoolingLowTempRadiantVarFlow_FieldSet, CoilCoolingLowTempRadiantVarFlow>
    {
        private IB_CoilCoolingLowTempRadiantVarFlow_FieldSet() { }

    }


}
