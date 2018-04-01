using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingElectric : IB_Coil
    {
        private static CoilHeatingElectric InitMethod(Model model) => new CoilHeatingElectric(model);

        public IB_CoilHeatingElectric() : base(InitMethod(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilHeatingElectric)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_CoilHeatingElectric());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_CoilHeatingElectric().get();
        }
    }

    public class IB_CoilHeatingElectric_DataFieldSet 
        : IB_DataFieldSet<IB_CoilHeatingElectric_DataFieldSet , CoilHeatingElectric>
    {
        private IB_CoilHeatingElectric_DataFieldSet() {}
    }
}