using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.HVAC
{
    public class IB_AirConditionerVariableRefrigerantFlow: IB_VRFSystem
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirConditionerVariableRefrigerantFlow();

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
        

        public override IB_ModelObject Duplicate()
        {
            var newObj = (IB_AirConditionerVariableRefrigerantFlow)base.DuplicateIBObj(IB_InitSelf);
            foreach (var item in this.Terminals)
            {
                newObj.AddTerminal((IB_ZoneHVACTerminalUnitVariableRefrigerantFlow)item.Duplicate());
            }

            return newObj;
        }

        protected override ModelObject InitOpsObj(Model model)
        {
            var newObj = base.OnInitOpsObj(InitMethod, model).to_AirConditionerVariableRefrigerantFlow().get();

            //TODO: how do I remind myself terminal is puppetable object

            //var termsTobeAdded = new List<IB_ZoneHVACTerminalUnitVariableRefrigerantFlow>();
            //foreach (var item in this.Terminals)
            //{
            //    if (item.Puppets.Count>0) //TODO: find a better way to do this. don't check puppets in downstream.
            //    {
            //        var puppets = item.Puppets.Select(_ => (IB_ZoneHVACTerminalUnitVariableRefrigerantFlow)_);
            //        termsTobeAdded.AddRange(puppets);
            //    }
            //    else
            //    {
            //        termsTobeAdded.Add(item);
            //    }

            //}
            var allTerms = this.Terminals.SelectMany(_ => _.GetPuppetsOrSelf());
            foreach (var terminal in allTerms)
            {
                
                var item = (IB_ZoneHVACTerminalUnitVariableRefrigerantFlow)terminal;
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
