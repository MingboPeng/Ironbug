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

        private CoilCoolingLowTempRadiantVarFlow NewDefaultOpsObj(Model model)
            => new CoilCoolingLowTempRadiantVarFlow(model, new ScheduleRuleset(model, airHiT));

        public new CoilCoolingLowTempRadiantVarFlow ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }

        public IB_CoilCoolingLowTempRadiantVarFlow( double airHiT) 
            : base(NewDefaultOpsObj(new Model(), airHiT))
        {
            this.airHiT = airHiT;
        }

    }

    public sealed class IB_CoilCoolingLowTempRadiantVarFlow_DataFieldSet
        : IB_FieldSet<IB_CoilCoolingLowTempRadiantVarFlow_DataFieldSet, CoilCoolingLowTempRadiantVarFlow>
    {
        private IB_CoilCoolingLowTempRadiantVarFlow_DataFieldSet() { }

    }


}
