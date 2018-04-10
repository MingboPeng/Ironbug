using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACTerminalUnitVariableRefrigerantFlow : IB_HVACObject
    {
        private static ZoneHVACTerminalUnitVariableRefrigerantFlow InitMethod(Model model) 
            => new ZoneHVACTerminalUnitVariableRefrigerantFlow(model);

        public IB_ZoneHVACTerminalUnitVariableRefrigerantFlow() : base(InitMethod(new Model()))
        { 
        }

        public override bool AddToNode(Node node)
        {
            var model = node.model();
            return ((ZoneHVACTerminalUnitVariableRefrigerantFlow)this.ToOS(model)).addToNode(node);
        }

        public override IB_ModelObject Duplicate()
        {
            return base.DuplicateIBObj(() => new IB_ZoneHVACTerminalUnitVariableRefrigerantFlow());
        }

        public override ModelObject ToOS(Model model)
        {
            return base.ToOS(InitMethod, model).to_ZoneHVACTerminalUnitVariableRefrigerantFlow().get();
        }
    }

    public sealed class IB_ZoneHVACTerminalUnitVariableRefrigerantFlow_DataFieldSet
        : IB_DataFieldSet<IB_ZoneHVACTerminalUnitVariableRefrigerantFlow_DataFieldSet, ZoneHVACTerminalUnitVariableRefrigerantFlow>
    {
        private IB_ZoneHVACTerminalUnitVariableRefrigerantFlow_DataFieldSet() { }
        
        //public IB_DataField Name { get; }
        //    = new IB_BasicDataField("Name", "Name");
        //public IB_DataField RatedCoolingCOP { get; }
        //    = new IB_BasicDataField("RatedCoolingCOP", "CoCOP");
        //public IB_DataField RatedHeatingCOP { get; }
        //    = new IB_BasicDataField("RatedHeatingCOP", "HeCOP");
    }
}
