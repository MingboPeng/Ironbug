using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
namespace Ironbug.HVAC
{
    public class IB_CoilHeatingDXVariableRefrigerantFlow : IB_CoilHeatingBasic
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingDXVariableRefrigerantFlow();
        private static CoilHeatingDXVariableRefrigerantFlow NewDefaultOpsObj(Model model) 
            => new CoilHeatingDXVariableRefrigerantFlow(model);
        
        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
        

        public IB_CoilHeatingDXVariableRefrigerantFlow() 
            : base(NewDefaultOpsObj(new Model()))
        {
        }

    }

    public sealed class IB_CoilHeatingDXVariableRefrigerantFlow_FieldSet
        : IB_FieldSet<IB_CoilHeatingDXVariableRefrigerantFlow_FieldSet, CoilHeatingDXVariableRefrigerantFlow>
    {
        private IB_CoilHeatingDXVariableRefrigerantFlow_FieldSet() { }

    }


}
