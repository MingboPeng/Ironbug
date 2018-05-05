using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System.Collections.Generic;

namespace Ironbug.HVAC
{
    public class IB_AirConditionerVariableRefrigerantFlow: IB_VRFSystem
    {
        public List<IB_ZoneHVACTerminalUnitVariableRefrigerantFlow> Terminals { get; private set; } 
            = new List<IB_ZoneHVACTerminalUnitVariableRefrigerantFlow>();

        private static AirConditionerVariableRefrigerantFlow InitMethod(Model model) 
            => new AirConditionerVariableRefrigerantFlow(model);

        public IB_AirConditionerVariableRefrigerantFlow() : base(InitMethod(new Model()))
        {
        }

        public void AddTerminal(IB_ZoneHVACTerminalUnitVariableRefrigerantFlow Terminal)
        {
            this.Terminals.Add(Terminal);
        }

        //public override bool AddToNode(Node node)
        //{
        //    var model = node.model();
        //    return ((AirConditionerVariableRefrigerantFlow)this.ToOS(model)).addToNode(node);
        //}

        public override IB_ModelObject Duplicate()
        {
            var newObj = (IB_AirConditionerVariableRefrigerantFlow)base.DuplicateIBObj(() => new IB_AirConditionerVariableRefrigerantFlow());
            foreach (var item in this.Terminals)
            {
                newObj.AddTerminal((IB_ZoneHVACTerminalUnitVariableRefrigerantFlow)item.Duplicate());
            }

            return newObj;
        }

        public override ModelObject ToOS(Model model)
        {
            var newObj = base.ToOS(InitMethod, model).to_AirConditionerVariableRefrigerantFlow().get();
            foreach (var item in this.Terminals)
            {
                newObj.addTerminal((ZoneHVACTerminalUnitVariableRefrigerantFlow)item.ToOS(model));
            }
            
            return newObj;
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
