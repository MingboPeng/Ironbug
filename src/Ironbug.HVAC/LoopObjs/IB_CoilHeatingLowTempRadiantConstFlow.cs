using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingLowTempRadiantConstFlow : IB_CoilBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        private double waterHiT = 50; //122F
        private double waterLoT = 30; //86F
        private double airHiT = 20; //68F
        private double airLoT = 17; //62.6F


        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingLowTempRadiantConstFlow(waterHiT, waterLoT, airHiT, airLoT);

        private static CoilHeatingLowTempRadiantConstFlow InitMethod(Model model, double waterHiT, double waterLoT, double airHiT, double airLoT) 
            => new CoilHeatingLowTempRadiantConstFlow(model, new ScheduleRuleset(model, waterHiT), new ScheduleRuleset(model, waterLoT), new ScheduleRuleset(model, airHiT), new ScheduleRuleset(model, airLoT));

        private CoilHeatingLowTempRadiantConstFlow InitMethod(Model model)
            => new CoilHeatingLowTempRadiantConstFlow(model, new ScheduleRuleset(model, waterHiT), new ScheduleRuleset(model, waterLoT), new ScheduleRuleset(model, airHiT), new ScheduleRuleset(model, airLoT));

        public new CoilHeatingLowTempRadiantConstFlow ToOS(Model model)
        {
            return (CoilHeatingLowTempRadiantConstFlow)base.ToOS(model);
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilHeatingLowTempRadiantConstFlow)this.ToOS(model)).addToNode(node);
        }
        
        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CoilHeatingLowTempRadiantConstFlow().get();
        }

        public IB_CoilHeatingLowTempRadiantConstFlow(double waterHiT, double waterLoT, double airHiT, double airLoT) 
            : base(InitMethod(new Model(), waterHiT, waterLoT, airHiT, airLoT))
        {
            this.airHiT = airHiT;
            this.airLoT = airLoT;
            this.waterLoT = waterLoT;
            this.waterHiT = waterHiT;
        }

    }

    public sealed class IB_CoilHeatingLowTempRadiantConstFlow_DataFieldSet
        : IB_FieldSet<IB_CoilHeatingLowTempRadiantConstFlow_DataFieldSet, CoilHeatingLowTempRadiantConstFlow>
    {
        private IB_CoilHeatingLowTempRadiantConstFlow_DataFieldSet() { }

    }


}
