using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Ironbug.HVAC
{
    public class IB_HVACScenario
    {
        [DataMember]
        public List<IB_HVACSystem> HVACSystems { get; private set; }
        [DataMember]
        public string DisplayName { get; private set; }
        [DataMember]
        public string Identifier { get; private set; }

        [DataMember]
        public string Info { get; private set; }

        private IB_HVACScenario()
        {
            this.HVACSystems = new List<IB_HVACSystem>();
        }

        public IB_HVACScenario(string id, string name, List<IB_HVACSystem> systems )
        {
            this.Identifier = string.IsNullOrEmpty(id) ? System.Guid.NewGuid().ToString().Substring(0, 6) : id;
            this.DisplayName = string.IsNullOrEmpty(name) ? this.Identifier : name;
            this.HVACSystems = systems.Where(_ => _ != null).ToList();
            this.Info = GetInfo();
        }

        public IB_HVACSystem CombineToHVACSystem()
        {
            var als = this.HVACSystems.SelectMany(_=>_.AirLoops).ToList();
            var pls = this.HVACSystems.SelectMany( _=>_.PlantLoops).ToList();
            var vrfs = this.HVACSystems.SelectMany(_=>_.VariableRefrigerantFlows).ToList();
            var sys = new IB_HVACSystem(als, pls, vrfs);
            return sys;
        }

        public string GetInfo()
        {
            if (HVACSystems == null || !HVACSystems.Any()) return string.Empty;

            var info = new List<string>();
            info.Add($"HVAC Scenario: {this.DisplayName} [{this.Identifier}]");
            for (int i = 0; i < HVACSystems.Count; i++)
            {
                var sys = HVACSystems[i];
                // system name
                var sysName = $"- {i + 1}: {sys.ToString()}";
                info.Add(sysName);
            }

            return string.Join(Environment.NewLine, info);
        }

        public override string ToString()
        {

            return this.Info?? base.ToString(); 
        }
    }
}
