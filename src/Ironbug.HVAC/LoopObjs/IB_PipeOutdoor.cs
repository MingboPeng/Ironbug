using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_PipeOutdoor : IB_HVACObject, IIB_PlantLoopObjects
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_PipeOutdoor();
        private static PipeOutdoor NewDefaultOpsObj(Model model)
            => new PipeOutdoor(model);
        

        public IB_PipeOutdoor():base(NewDefaultOpsObj(new Model()))
        {
        }
        protected override ModelObject NewOpsObj(Model model)
        {
            return base.OnNewOpsObj(NewDefaultOpsObj, model).to_PipeOutdoor().get();
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((PipeOutdoor)base.ToOS(model)).addToNode(node);
        }
    }

    public sealed class IB_PipeOutdoor_FieldSet
        : IB_FieldSet<IB_PipeOutdoor_FieldSet, PipeOutdoor>
    {
        private IB_PipeOutdoor_FieldSet() { }
        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");

    }
}
