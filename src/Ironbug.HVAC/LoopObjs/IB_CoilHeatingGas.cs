using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingGas : IB_CoilBasic
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingGas();

        private static CoilHeatingGas InitMethod(Model model) => new CoilHeatingGas(model);

        public IB_CoilHeatingGas() : base(InitMethod(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilHeatingGas)this.ToOS(model)).addToNode(node);
        }
        

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CoilHeatingGas().get();
        }
    }

    public sealed class IB_CoilCoolingGas_DataFieldSet 
        : IB_FieldSet<IB_CoilCoolingGas_DataFieldSet, CoilHeatingGas>
    {
        private IB_CoilCoolingGas_DataFieldSet() {}
    }
}
