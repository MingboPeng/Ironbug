using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingDXSingleSpeed : IB_Coil
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilCoolingDXSingleSpeed();

        private static CoilCoolingDXSingleSpeed InitMethod(Model model) => new CoilCoolingDXSingleSpeed(model);

        public IB_CoilCoolingDXSingleSpeed() : base(InitMethod(new Model()))
        {
            
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilCoolingDXSingleSpeed)this.ToOS(model)).addToNode(node);
        }

        //public override IB_ModelObject Duplicate()
        //{
        //    return base.DuplicateIBObj(() => new IB_CoilCoolingDXSingleSpeed());
        //}

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CoilCoolingDXSingleSpeed().get();
        }
    }

    public sealed class IB_CoilCoolingDXSingleSpeed_DataFieldSet
        : IB_DataFieldSet<IB_CoilCoolingDXSingleSpeed_DataFieldSet, CoilCoolingDXSingleSpeed>
    {
        private IB_CoilCoolingDXSingleSpeed_DataFieldSet() { }

    }
}
