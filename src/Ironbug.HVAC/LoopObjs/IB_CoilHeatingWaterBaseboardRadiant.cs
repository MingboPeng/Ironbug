using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingWaterBaseboardRadiant : IB_CoilHeatingBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingWaterBaseboardRadiant();

        private static CoilHeatingWaterBaseboardRadiant NewDefaultOpsObj(Model model) => new CoilHeatingWaterBaseboardRadiant(model);
        
        public IB_CoilHeatingWaterBaseboardRadiant() : base(NewDefaultOpsObj(new Model()))
        {
        }
        
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilHeatingWaterBaseboardRadiant)this.NewOpsObj(model)).addToNode(node);

        }


        protected override ModelObject NewOpsObj(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model).to_CoilHeatingWaterBaseboardRadiant().get();
        }


    }

    public sealed class IB_CoilHeatingWaterBaseboardRadiant_DataFieldSet
        : IB_FieldSet<IB_CoilHeatingWaterBaseboardRadiant_DataFieldSet, CoilHeatingWaterBaseboardRadiant>
    {
        private IB_CoilHeatingWaterBaseboardRadiant_DataFieldSet() {}
        
    }
    
    

}
