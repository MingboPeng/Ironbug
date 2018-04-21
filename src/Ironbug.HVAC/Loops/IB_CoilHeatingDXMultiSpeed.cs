using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingDXMultiSpeed : IB_Coil
    {
        private static CoilHeatingDXMultiSpeed InitMethod(Model model) => new CoilHeatingDXMultiSpeed(model);

        public IB_CoilHeatingDXMultiSpeed() : base(InitMethod(new Model()))
        {
            
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilHeatingDXMultiSpeed)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_CoilHeatingDXMultiSpeed());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_CoilHeatingDXMultiSpeed().get();
        }

    }

    public sealed class IB_CoilHeatingDXMultiSpeed_DataFieldSet
        : IB_DataFieldSet<IB_CoilHeatingDXMultiSpeed_DataFieldSet, CoilHeatingDXMultiSpeed>
    {
        private IB_CoilHeatingDXMultiSpeed_DataFieldSet() { }

    }
}
