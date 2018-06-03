using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingDXMultiSpeed : IB_CoilDX
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingDXMultiSpeed();

        private static CoilHeatingDXMultiSpeed InitMethod(Model model) => new CoilHeatingDXMultiSpeed(model);

        public IB_CoilHeatingDXMultiSpeed() : base(InitMethod(new Model()))
        {
            
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilHeatingDXMultiSpeed)this.ToOS(model)).addToNode(node);
        }
        

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CoilHeatingDXMultiSpeed().get();
        }

    }

    public sealed class IB_CoilHeatingDXMultiSpeed_DataFieldSet
        : IB_FieldSet<IB_CoilHeatingDXMultiSpeed_DataFieldSet, CoilHeatingDXMultiSpeed>
    {
        private IB_CoilHeatingDXMultiSpeed_DataFieldSet() { }

    }
}
