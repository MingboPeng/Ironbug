using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingDXTwoSpeed : IB_CoilDX
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingDXTwoSpeed();

        private static CoilCoolingDXTwoSpeed NewDefaultOpsObj(Model model) => new CoilCoolingDXTwoSpeed(model);

        public IB_CoilCoolingDXTwoSpeed() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilCoolingDXTwoSpeed)this.ToOS(model)).addToNode(node);
        }

        //public override IB_ModelObject Duplicate()
        //{
        //    return base.DuplicateIBObj(() => new IB_CoilCoolingDXTwoSpeed());
        //}

        protected override ModelObject NewOpsObj(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model).to_CoilCoolingDXTwoSpeed().get();
        }
    }

    public sealed class IB_CoilCoolingDXTwoSpeed_DataFieldSet
        : IB_FieldSet<IB_CoilCoolingDXTwoSpeed_DataFieldSet, CoilCoolingDXTwoSpeed>
    {
        private IB_CoilCoolingDXTwoSpeed_DataFieldSet() { }

    }
}
