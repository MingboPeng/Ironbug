using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_AirConditionerVariableRefrigerantFlow: IB_HVACObject
    {
        private static AirConditionerVariableRefrigerantFlow InitMethod(Model model) => new AirConditionerVariableRefrigerantFlow(model);

        public IB_AirConditionerVariableRefrigerantFlow() : base(InitMethod(new Model()))
        {
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((AirConditionerVariableRefrigerantFlow)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_AirConditionerVariableRefrigerantFlow());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_AirConditionerVariableRefrigerantFlow().get();
        }
    }

    public sealed class IB_AirConditionerVariableRefrigerantFlow_DataFieldSet
        : IB_DataFieldSet<IB_AirConditionerVariableRefrigerantFlow_DataFieldSet, AirConditionerVariableRefrigerantFlow>
    {
        private IB_AirConditionerVariableRefrigerantFlow_DataFieldSet() { }
        
        public IB_DataField Name { get; }
            = new IB_BasicDataField("Name", "Name");
        public IB_DataField RatedCoolingCOP { get; }
            = new IB_BasicDataField("RatedCoolingCOP", "CoCOP");
        public IB_DataField RatedHeatingCOP { get; }
            = new IB_BasicDataField("RatedHeatingCOP", "HeCOP");
    }
}
