using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_Duct : IB_HVACObject, IIB_AirLoopObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_Duct();
        private static Duct InitMethod(Model model)
            => new Duct(model);
        

        public IB_Duct():base(InitMethod(new Model()))
        {
        }
        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_Duct().get();
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((Duct)base.ToOS(model)).addToNode(node);
        }
    }

    public sealed class IB_Duct_FieldSet
        : IB_FieldSet<IB_Duct_FieldSet, Duct>
    {
        private IB_Duct_FieldSet() { }
        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");

    }
}
