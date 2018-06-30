using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingWaterBaseboard : IB_CoilBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingWaterBaseboard();

        private static CoilHeatingWaterBaseboard InitMethod(Model model) => new CoilHeatingWaterBaseboard(model);
        
        public IB_CoilHeatingWaterBaseboard() : base(InitMethod(new Model()))
        {
        }
        
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilHeatingWaterBaseboard)this.InitOpsObj(model)).addToNode(node);

        }


        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CoilHeatingWaterBaseboard().get();
        }


    }

    public sealed class IB_CoilHeatingWaterBaseboard_DataFieldSet
        : IB_FieldSet<IB_CoilHeatingWaterBaseboard_DataFieldSet, CoilHeatingWaterBaseboard>
    {
        private IB_CoilHeatingWaterBaseboard_DataFieldSet() {}
        
    }
    
    

}
