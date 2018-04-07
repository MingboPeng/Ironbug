using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_CoilCoolingDXSingleSpeed : IB_Coil
    {
        private static CoilCoolingDXSingleSpeed InitMethod(Model model) => new CoilCoolingDXSingleSpeed(model);

        public IB_CoilCoolingDXSingleSpeed() : base(InitMethod(new Model()))
        {
            
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilCoolingDXSingleSpeed)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_CoilCoolingDXSingleSpeed());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_CoilCoolingDXSingleSpeed().get();
        }
    }

    public sealed class IB_CoilCoolingDXSingleSpeed_DataFieldSet
        : IB_DataFieldSet<IB_CoilCoolingDXSingleSpeed_DataFieldSet, CoilCoolingDXSingleSpeed>
    {
        private IB_CoilCoolingDXSingleSpeed_DataFieldSet() { }

    }
}
