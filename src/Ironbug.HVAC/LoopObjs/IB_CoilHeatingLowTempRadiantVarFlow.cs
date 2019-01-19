using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingLowTempRadiantVarFlow : IB_CoilHeatingBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        private double airLoT = 15;
        
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingLowTempRadiantVarFlow(airLoT);

        private static CoilHeatingLowTempRadiantVarFlow InitMethod(Model model, double airLoT) 
            => new CoilHeatingLowTempRadiantVarFlow(model, new ScheduleRuleset(model, airLoT));

        private CoilHeatingLowTempRadiantVarFlow InitMethod(Model model)
            => new CoilHeatingLowTempRadiantVarFlow(model, new ScheduleRuleset(model, airLoT));

        public new CoilHeatingLowTempRadiantVarFlow ToOS(Model model)
        {
            return (CoilHeatingLowTempRadiantVarFlow)base.ToOS(model);
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return this.ToOS(model).addToNode(node);
        }
        
        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CoilHeatingLowTempRadiantVarFlow().get();
        }

        public IB_CoilHeatingLowTempRadiantVarFlow(double airLoT) 
            : base(InitMethod(new Model(), airLoT))
        {
            this.airLoT = airLoT;
        }

    }

    public sealed class IB_CoilHeatingLowTempRadiantVarFlow_DataFieldSet
        : IB_FieldSet<IB_CoilHeatingLowTempRadiantVarFlow_DataFieldSet, CoilHeatingLowTempRadiantVarFlow>
    {
        private IB_CoilHeatingLowTempRadiantVarFlow_DataFieldSet() { }

    }


}
