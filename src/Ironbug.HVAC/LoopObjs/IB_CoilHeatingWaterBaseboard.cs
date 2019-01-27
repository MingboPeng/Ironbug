using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingWaterBaseboard : IB_CoilHeatingBasic, IIB_DualLoopObj, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingWaterBaseboard();

        private static CoilHeatingWaterBaseboard NewDefaultOpsObj(Model model) => new CoilHeatingWaterBaseboard(model);
        
        public IB_CoilHeatingWaterBaseboard() : base(NewDefaultOpsObj(new Model()))
        {
        }
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }


    }

    public sealed class IB_CoilHeatingWaterBaseboard_DataFieldSet
        : IB_FieldSet<IB_CoilHeatingWaterBaseboard_DataFieldSet, CoilHeatingWaterBaseboard>
    {
        private IB_CoilHeatingWaterBaseboard_DataFieldSet() {}
        
    }
    
    

}
