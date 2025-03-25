using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_HeatExchangerDesiccantBalancedFlow : IB_HVACObject, IIB_AirLoopObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_HeatExchangerDesiccantBalancedFlow();

        private static HeatExchangerDesiccantBalancedFlow NewDefaultOpsObj(Model model) => new HeatExchangerDesiccantBalancedFlow(model);

        public IB_HeatExchangerDesiccantBalancedFlow() : base(NewDefaultOpsObj)
        {

        }


        public override HVACComponent ToOS(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model);
        }
    }

    public sealed class IB_HeatExchangerDesiccantBalancedFlow_FieldSet
        : IB_FieldSet<IB_HeatExchangerDesiccantBalancedFlow_FieldSet, HeatExchangerDesiccantBalancedFlow>
    {
        private IB_HeatExchangerDesiccantBalancedFlow_FieldSet() { }
    }
}