using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingLowTempRadiantConstFlow : IB_CoilBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        private double waterHiT = 15; //59F
        private double waterLoT = 10; //50F
        private double airHiT = 25; //77F
        private double airLoT = 21; //70F


        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingLowTempRadiantConstFlow(waterHiT, waterLoT, airHiT, airLoT);

        private static CoilCoolingLowTempRadiantConstFlow InitMethod(Model model, double waterHiT, double waterLoT, double airHiT, double airLoT) 
            => new CoilCoolingLowTempRadiantConstFlow(model, new ScheduleRuleset(model, waterHiT), new ScheduleRuleset(model, waterLoT), new ScheduleRuleset(model, airHiT), new ScheduleRuleset(model, airLoT));

        private CoilCoolingLowTempRadiantConstFlow InitMethod(Model model)
            => new CoilCoolingLowTempRadiantConstFlow(model, new ScheduleRuleset(model, waterHiT), new ScheduleRuleset(model, waterLoT), new ScheduleRuleset(model, airHiT), new ScheduleRuleset(model, airLoT));

        public new CoilCoolingLowTempRadiantConstFlow ToOS(Model model)
        {
            return (CoilCoolingLowTempRadiantConstFlow)base.ToOS(model);
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilCoolingLowTempRadiantConstFlow)this.ToOS(model)).addToNode(node);
        }
        
        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CoilCoolingLowTempRadiantConstFlow().get();
        }

        public IB_CoilCoolingLowTempRadiantConstFlow(double waterHiT, double waterLoT, double airHiT, double airLoT) 
            : base(InitMethod(new Model(), waterHiT, waterLoT, airHiT, airLoT))
        {
            this.airHiT = airHiT;
            this.airLoT = airLoT;
            this.waterLoT = waterLoT;
            this.waterHiT = waterHiT;
        }

    }

    public sealed class IB_CoilCoolingLowTempRadiantConstFlow_DataFieldSet
        : IB_FieldSet<IB_CoilCoolingLowTempRadiantConstFlow_DataFieldSet, CoilCoolingLowTempRadiantConstFlow>
    {
        private IB_CoilCoolingLowTempRadiantConstFlow_DataFieldSet() { }

    }


}
