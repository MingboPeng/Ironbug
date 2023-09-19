﻿using Ironbug.HVAC.BaseClass;
using OpenStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Ironbug.HVAC
{
    public class IB_AirConditionerVariableRefrigerantFlow: IB_VRFSystem, IEquatable<IB_AirConditionerVariableRefrigerantFlow>
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_AirConditionerVariableRefrigerantFlow();

        [DataMember]
        public List<IB_ZoneHVACTerminalUnitVariableRefrigerantFlow> Terminals { get; private set; } 
            = new List<IB_ZoneHVACTerminalUnitVariableRefrigerantFlow>();

        

        private static AirConditionerVariableRefrigerantFlow NewDefaultOpsObj(Model model) 
            => new AirConditionerVariableRefrigerantFlow(model);

        public IB_AirConditionerVariableRefrigerantFlow() : base(NewDefaultOpsObj(new Model()))
        {
        }

        public void AddTerminal(IB_ZoneHVACTerminalUnitVariableRefrigerantFlow Terminal)
        {
            this.Terminals.Add(Terminal);
        }
        

        public override IB_ModelObject Duplicate()
        {
            var newObj = base.Duplicate(() => new IB_AirConditionerVariableRefrigerantFlow());
            foreach (var item in this.Terminals)
            {
                newObj.AddTerminal((IB_ZoneHVACTerminalUnitVariableRefrigerantFlow)item.Duplicate());
            }

            return newObj;
        }
        public override HVACComponent ToOS(Model model)
        {
            var existObj = this.GetIfInModel<AirConditionerVariableRefrigerantFlow>(model, this.GetTrackingID());
            if (existObj != null) return existObj;
            
            var newObj = base.OnNewOpsObj(NewDefaultOpsObj, model);

            var allTerms = this.Terminals;
            foreach (var terminal in allTerms)
            {
                var t = terminal.ToOS(model) as ZoneHVACTerminalUnitVariableRefrigerantFlow;
                newObj.addTerminal(t);
                
            }
            
            return newObj;
        }

        public override bool Equals(object obj) => this.Equals(obj as IB_AirConditionerVariableRefrigerantFlow);
        public bool Equals(IB_AirConditionerVariableRefrigerantFlow other)
        {
            if (!base.Equals(other))
                return false;

            if (!this.Terminals.SequenceEqual(other.Terminals))
                return false;

            return true;
        }
    }

    public sealed class IB_AirConditionerVariableRefrigerantFlow_FieldSet
        : IB_FieldSet<IB_AirConditionerVariableRefrigerantFlow_FieldSet, AirConditionerVariableRefrigerantFlow>
    {
        private IB_AirConditionerVariableRefrigerantFlow_FieldSet() { }
        
        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");
        public IB_Field RatedCoolingCOP { get; }
            = new IB_BasicField("RatedCoolingCOP", "CoCOP");
        public IB_Field RatedHeatingCOP { get; }
            = new IB_BasicField("RatedHeatingCOP", "HeCOP");

        public IB_Field BasinHeaterOperatingSchedule { get; } 
            = new IB_Field("BasinHeaterOperatingSchedule", "BasinHeaterOperatingSchedule");

        public IB_Field CoolingCapacityRatioBoundaryCurve { get; }
            = new IB_Field("CoolingCapacityRatioBoundaryCurve", "CoolingCapacityRatioBoundaryCurve");
    }
}
