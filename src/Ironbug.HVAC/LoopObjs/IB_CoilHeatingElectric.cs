using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_CoilHeatingElectric : IB_CoilBasic
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_CoilHeatingElectric();

        private static CoilHeatingElectric InitMethod(Model model) => new CoilHeatingElectric(model);

        public IB_CoilHeatingElectric() : base(InitMethod(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((CoilHeatingElectric)this.ToOS(model)).addToNode(node);
        }
        

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_CoilHeatingElectric().get();
        }
    }

    public sealed class IB_CoilHeatingElectric_DataFieldSet 
        : IB_FieldSet<IB_CoilHeatingElectric_DataFieldSet , CoilHeatingElectric>
    {
        private IB_CoilHeatingElectric_DataFieldSet() {}
    }
}