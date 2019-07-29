using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
namespace Ironbug.HVAC
{
    public class IB_CoilCoolingDXVariableRefrigerantFlow : IB_CoilCoolingBasic
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingDXVariableRefrigerantFlow();
        private static CoilCoolingDXVariableRefrigerantFlow NewDefaultOpsObj(Model model) 
            => new CoilCoolingDXVariableRefrigerantFlow(model);
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        

        public IB_CoilCoolingDXVariableRefrigerantFlow() 
            : base(NewDefaultOpsObj(new Model()))
        {
        }

    }

    public sealed class IB_CoilCoolingDXVariableRefrigerantFlow_FieldSet
        : IB_FieldSet<IB_CoilCoolingDXVariableRefrigerantFlow_FieldSet, CoilCoolingDXVariableRefrigerantFlow>
    {
        private IB_CoilCoolingDXVariableRefrigerantFlow_FieldSet() { }

    }


}
