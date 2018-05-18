using System;
using System.Collections.Generic;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACTerminalUnitVariableRefrigerantFlow 
        : IB_ZoneEquipment, IIB_ShareableObj
        
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ZoneHVACTerminalUnitVariableRefrigerantFlow();

        private static ZoneHVACTerminalUnitVariableRefrigerantFlow InitMethod(Model model) 
            => new ZoneHVACTerminalUnitVariableRefrigerantFlow(model);
        
        public IB_ZoneHVACTerminalUnitVariableRefrigerantFlow() : base(InitMethod(new Model()))
        { 
        }
        
        //TODO: maybe I need to duplicate the puppets as well, but maybe not.
        //probably not, because puppets can only have one parent, and each replica have to create their own puppet again.
        //public override IB_ModelObject Duplicate()
        //{
        //    return base.DuplicateIBObj(() => new IB_ZoneHVACTerminalUnitVariableRefrigerantFlow());
        //}

        protected override ModelObject InitOpsObj(Model model)
        {
            //TODO: double check this new way
            return base.OnInitOpsObj(InitMethod, model,(_)=>_.to_ZoneHVACTerminalUnitVariableRefrigerantFlow().get());
            //return base.OnInitOpsObj(InitMethod, model).to_ZoneHVACTerminalUnitVariableRefrigerantFlow().get();
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
