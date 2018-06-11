using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingWater : IB_CoilBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingWater();

        private static CoilCoolingWater InitMethod(Model model) => new CoilCoolingWater(model);

        public IB_CoilCoolingWater() : base(InitMethod(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilCoolingWater)this.ToOS(model)).addToNode(node);
        }
        

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CoilCoolingWater().get();
        }
    }
    public sealed class IB_CoilCoolingWater_DataFieldSet 
        : IB_FieldSet<IB_CoilCoolingWater_DataFieldSet, CoilCoolingWater>
    {
        private IB_CoilCoolingWater_DataFieldSet() { }
        
    }

}
