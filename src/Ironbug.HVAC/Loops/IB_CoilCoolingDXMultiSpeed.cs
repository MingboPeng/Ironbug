using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingDXMultiSpeed : IB_Coil
    {
        private static CoilCoolingDXMultiSpeed InitMethod(Model model) => new CoilCoolingDXMultiSpeed(model);

        public IB_CoilCoolingDXMultiSpeed() : base(InitMethod(new Model()))
        {
            
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilCoolingDXMultiSpeed)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_CoilCoolingDXMultiSpeed());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_CoilCoolingDXMultiSpeed().get();
        }

    }

    public sealed class IB_CoilCoolingDXMultiSpeed_DataFieldSet
        : IB_DataFieldSet<IB_CoilCoolingDXMultiSpeed_DataFieldSet, CoilCoolingDXMultiSpeed>
    {
        private IB_CoilCoolingDXMultiSpeed_DataFieldSet() { }

    }
}
