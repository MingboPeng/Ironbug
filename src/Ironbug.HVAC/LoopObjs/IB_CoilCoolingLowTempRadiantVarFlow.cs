using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingLowTempRadiantVarFlow : IB_CoilBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        private double airHiT = 50; 
        
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingLowTempRadiantVarFlow( airHiT);

        private static CoilCoolingLowTempRadiantVarFlow InitMethod(Model model, double airHiT) 
            => new CoilCoolingLowTempRadiantVarFlow(model, new ScheduleRuleset(model, airHiT));

        private CoilCoolingLowTempRadiantVarFlow InitMethod(Model model)
            => new CoilCoolingLowTempRadiantVarFlow(model, new ScheduleRuleset(model, airHiT));

        public new CoilCoolingLowTempRadiantVarFlow ToOS(Model model)
        {
            return (CoilCoolingLowTempRadiantVarFlow)base.ToOS(model);
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return this.ToOS(model).addToNode(node);
        }
        
        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CoilCoolingLowTempRadiantVarFlow().get();
        }

        public IB_CoilCoolingLowTempRadiantVarFlow( double airHiT) 
            : base(InitMethod(new Model(), airHiT))
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
