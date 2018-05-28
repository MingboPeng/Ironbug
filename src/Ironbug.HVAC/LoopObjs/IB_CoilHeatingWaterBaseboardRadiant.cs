using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingWaterBaseboardRadiant : IB_CoilBasic, IIB_ShareableObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingWaterBaseboardRadiant();

        private static CoilHeatingWaterBaseboardRadiant InitMethod(Model model) => new CoilHeatingWaterBaseboardRadiant(model);
        
        public IB_CoilHeatingWaterBaseboardRadiant() : base(InitMethod(new Model()))
        {
        }
        
        public override bool AddToNode(Node node)
        {
            //this is only used in IB_ZoneHVACBaseboardRadiantConvectiveWater
            return true;
            
        }


        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CoilHeatingWaterBaseboardRadiant().get();
        }


    }

    public sealed class IB_CoilHeatingWaterBaseboardRadiant_DataFieldSet
        : IB_DataFieldSet<IB_CoilHeatingWaterBaseboardRadiant_DataFieldSet, CoilHeatingWaterBaseboardRadiant>
    {
        private IB_CoilHeatingWaterBaseboardRadiant_DataFieldSet() {}
        
    }
    
    

}
