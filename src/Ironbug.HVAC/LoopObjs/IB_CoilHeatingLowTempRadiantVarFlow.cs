﻿using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingLowTempRadiantVarFlow : IB_CoilHeatingBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        private double airLoT = 15;
        
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingLowTempRadiantVarFlow(airLoT);

        private static CoilHeatingLowTempRadiantVarFlow NewDefaultOpsObj(Model model, double airLoT) 
            => new CoilHeatingLowTempRadiantVarFlow(model, new ScheduleRuleset(model, airLoT));

        private CoilHeatingLowTempRadiantVarFlow NewDefaultOpsObj(Model model)
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
        
        protected override ModelObject NewOpsObj(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model).to_CoilHeatingLowTempRadiantVarFlow().get();
        }

        public IB_CoilHeatingLowTempRadiantVarFlow(double airLoT) 
            : base(NewDefaultOpsObj(new Model(), airLoT))
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