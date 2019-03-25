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
  
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }


    }

    public sealed class IB_CoilHeatingWaterBaseboardRadiant_FieldSet
        : IB_FieldSet<IB_CoilHeatingWaterBaseboardRadiant_FieldSet, CoilHeatingWaterBaseboardRadiant>
    {
        private IB_CoilHeatingWaterBaseboardRadiant_FieldSet() {}
        
    }
    
    

}
