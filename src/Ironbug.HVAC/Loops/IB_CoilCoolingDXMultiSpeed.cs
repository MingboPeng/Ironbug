using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingDXMultiSpeed : IB_Coil
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingDXMultiSpeed();

        private static CoilCoolingDXMultiSpeed InitMethod(Model model) => new CoilCoolingDXMultiSpeed(model);

        public IB_CoilCoolingDXMultiSpeed() : base(InitMethod(new Model()))
        {
            
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilCoolingDXMultiSpeed)this.ToOS(model)).addToNode(node);
        }

        //public override IB_ModelObject Duplicate()
        //{
        //    return base.DuplicateIBObj(() => new IB_CoilCoolingDXMultiSpeed());
        //}

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CoilCoolingDXMultiSpeed().get();
        }

    }

    public sealed class IB_CoilCoolingDXMultiSpeed_DataFieldSet
        : IB_DataFieldSet<IB_CoilCoolingDXMultiSpeed_DataFieldSet, CoilCoolingDXMultiSpeed>
    {
        private IB_CoilCoolingDXMultiSpeed_DataFieldSet() { }

    }
}
