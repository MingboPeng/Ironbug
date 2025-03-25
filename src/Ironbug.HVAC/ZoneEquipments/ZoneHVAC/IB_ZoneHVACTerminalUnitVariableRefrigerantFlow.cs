using System;
using Ironbug.HVAC.BaseClass;
using OpenStudio;

namespace Ironbug.HVAC
{
    public class IB_ZoneHVACTerminalUnitVariableRefrigerantFlow 
        : IB_ZoneEquipment, IIB_DualLoopObj, IIB_AirLoopObject

    {
        protected override Func<IB_ModelObject> IB_InitSelf => delegate ()
        {
            if (this.Children.Count > 0)
            {
                return new IB_ZoneHVACTerminalUnitVariableRefrigerantFlow(this._coolingCoil, this._heatingCoil, this._fan);
            }
            else
            {
                return new IB_ZoneHVACTerminalUnitVariableRefrigerantFlow();
            }
        };

        private static ZoneHVACTerminalUnitVariableRefrigerantFlow NewDefaultOpsObj(Model model) 
            => new ZoneHVACTerminalUnitVariableRefrigerantFlow(model);

        private IB_CoilCoolingDXVariableRefrigerantFlow _coolingCoil => this.GetChild<IB_CoilCoolingDXVariableRefrigerantFlow>();
        private IB_CoilHeatingDXVariableRefrigerantFlow _heatingCoil => this.GetChild<IB_CoilHeatingDXVariableRefrigerantFlow>();
        private IB_FanOnOff _fan => this.GetChild<IB_FanOnOff>();

        public IB_ZoneHVACTerminalUnitVariableRefrigerantFlow() : base(NewDefaultOpsObj)
        {
        }

        public IB_ZoneHVACTerminalUnitVariableRefrigerantFlow(IB_CoilCoolingDXVariableRefrigerantFlow CoolingCoil, IB_CoilHeatingDXVariableRefrigerantFlow HeatingCoil, IB_FanOnOff Fan) : base(NewDefaultOpsObj)
        {
            this.AddChild(CoolingCoil);
            this.AddChild(HeatingCoil);
            this.AddChild(Fan);
        }


        public override HVACComponent ToOS(Model model)
        {
            if (this.Children.Count == 0)
            {
                return base.OnNewOpsObj(NewDefaultOpsObj, model);
            }
            else
            {
                return base.OnNewOpsObj(NewOpsObj, model);
            }

            //Local Method
            ZoneHVACTerminalUnitVariableRefrigerantFlow NewOpsObj(Model m)
            {
                return new ZoneHVACTerminalUnitVariableRefrigerantFlow(m, 
                    this._coolingCoil.ToOS(m) as CoilCoolingDXVariableRefrigerantFlow, 
                    this._heatingCoil.ToOS(m) as CoilHeatingDXVariableRefrigerantFlow, 
                    this._fan.ToOS(m));
            }
        }



    }

    public sealed class IB_ZoneHVACTerminalUnitVariableRefrigerantFlow_FieldSet
        : IB_FieldSet<IB_ZoneHVACTerminalUnitVariableRefrigerantFlow_FieldSet, ZoneHVACTerminalUnitVariableRefrigerantFlow>
    {
        private IB_ZoneHVACTerminalUnitVariableRefrigerantFlow_FieldSet() { }

        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name");
    }
}
