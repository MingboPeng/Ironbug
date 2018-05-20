using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;

namespace Ironbug.HVAC
{
    public class IB_SetpointManagerWarmest : IB_SetpointManager
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_SetpointManagerWarmest();

        private static SetpointManagerWarmest InitMethod(Model model) => new SetpointManagerWarmest(model);
        public IB_SetpointManagerWarmest() : base(InitMethod(new Model()))
        {
            
        }
        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((SetpointManagerWarmest)this.ToOS(model)).addToNode(node);
        }
        

        protected override ModelObject InitOpsObj(Model model)
        {
            return base.OnInitOpsObj(InitMethod, model).to_SetpointManagerWarmest().get();
        }
    }

    public sealed class IB_SetpointManagerWarmest_DataFieldSet
        : IB_DataFieldSet<IB_SetpointManagerWarmest_DataFieldSet, SetpointManagerWarmest>
    {
        private IB_SetpointManagerWarmest_DataFieldSet() { }

        public IB_DataField MaximumSetpointTemperature { get; }
            = new IB_MandatoryDataField("MaximumSetpointTemperature", "maxTemp");

        public IB_DataField MinimumSetpointTemperature { get; }
            = new IB_MandatoryDataField("MinimumSetpointTemperature", "minTemp");
    }


}
