using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingDXTwoSpeed : IB_Coil
    {
        private static CoilCoolingDXTwoSpeed InitMethod(Model model) => new CoilCoolingDXTwoSpeed(model);

        public IB_CoilCoolingDXTwoSpeed() : base(InitMethod(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilCoolingDXTwoSpeed)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_CoilCoolingDXTwoSpeed());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_CoilCoolingDXTwoSpeed().get();
        }
    }

    public sealed class IB_CoilCoolingDXTwoSpeed_DataFieldSet
        : IB_DataFieldSet<IB_CoilCoolingDXTwoSpeed_DataFieldSet, CoilCoolingDXTwoSpeed>
    {
        private IB_CoilCoolingDXTwoSpeed_DataFieldSet() { }

    }
}
