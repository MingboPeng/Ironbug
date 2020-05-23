using System;
using System.Collections.Generic;
using System.Linq;
using OpenStudio;

namespace Ironbug.HVAC.BaseClass
{
    public class IB_ThermalZone : IB_HVACObject, IIB_AirLoopObject
    {
        protected override Func<IB_ModelObject> IB_InitSelf => () => new IB_ThermalZone();
        private static ThermalZone NewDefaultOpsObj(Model model) => new ThermalZone(model);

        public IB_AirTerminal AirTerminal { get; private set; } = new IB_AirTerminalSingleDuctConstantVolumeNoReheat();
        public List<IIB_ZoneEquipment> ZoneEquipments { get; private set; } = new List<IIB_ZoneEquipment>();
        private IB_SizingZone IB_SizingZone { get; set; } = new IB_SizingZone();

        public IB_ThermalZone SupplyPlenum { get; private set; }
        public IB_ThermalZone ReturnPlenum { get; private set; }

        public bool IsAirTerminalPriorToZoneEquipments { get; set; } = false;

        public IB_ThermalZone():base(NewDefaultOpsObj(new Model()))
        {
            
        }
        public IB_ThermalZone(string HBZoneName) : base(NewDefaultOpsObj(new Model()))
        {
            base.SetFieldValue(IB_ThermalZone_FieldSet.Value.Name, HBZoneName);

        }


        public ModelObject GetModelObject()
        {
            return base.GhostOSObject;
        }

        /// <summary>
        /// Do not use this 
        /// Use this method to add a new SizingZone to this ThermalZone;
        /// Or construct the IB_SizingZone by IB_SizingZone(IB_ThermalZone);
        /// </summary>
        /// <param name="NewSizingZone"></param>
        public void SetSizingZone(IB_SizingZone NewSizingZone)
        {
            this.IB_SizingZone = NewSizingZone;
        }

        
        public void SetAirTerminal(IB_AirTerminal AirTerminal)
        {
            this.AirTerminal = AirTerminal;
        }

        public void AddZoneEquipment(IIB_ZoneEquipment Equipment)
        {
            this.ZoneEquipments.Add(Equipment);
        }

        public void SetSupplyPlenum(IB_ThermalZone Plenum)
        {
            if (Plenum != null) this.SupplyPlenum = Plenum;
        }
        public void SetReturnPlenum(IB_ThermalZone Plenum)
        {
            if (Plenum != null) this.ReturnPlenum = Plenum;
        }

        public HVACComponent ToOS(Model model, AirLoopHVAC airLoop)
        {
            var newZone = (ThermalZone)this.ToOS(model);
            var airTerminal = this.AirTerminal.ToOS(model);

            if (this.IsAirTerminalPriorToZoneEquipments)
            {
                if (!airLoop.addBranchForZone(newZone, airTerminal))
                    throw new ArgumentException($"Failed to add thermal zone to {airLoop.nameString()}!");

                foreach (var item in this.ZoneEquipments)
                {
                    var eqp = (ZoneHVACComponent)item.ToOS(model);
                    if (!eqp.addToThermalZone(newZone))
                        throw new ArgumentException($"Failed to add {eqp.nameString()} to thermal zone {newZone.nameString()}!");
                }
            }
            else
            {
                foreach (var item in this.ZoneEquipments)
                {
                    var eqp = (ZoneHVACComponent)item.ToOS(model);
                    eqp.addToThermalZone(newZone);
                }

                if (!airLoop.addBranchForZone(newZone, airTerminal))
                    throw new ArgumentException($"Failed to add thermal zone to {airLoop.nameString()}!");
                
            }

            //check if it is Mixer
            if (AirTerminal is IB_AirTerminalSingleDuctInletSideMixer mixer)
            {
                var nd = ((AirTerminalSingleDuctInletSideMixer)airTerminal).outletModelObject().get().to_Node().get();

                var eqp = mixer.MixedZoneEquip.GetOsmObjInModel(model);
                if (eqp is ZoneHVACComponent zc)
                {
                    zc.addToNode(nd);
                }
                else
                {
                    throw new ArgumentException($"Failed to add {airTerminal.nameString()} with {mixer.MixedZoneEquip.GetType()} in thermal zone!");
                }
            }

            
            //add plenums
            if (this.SupplyPlenum != null) newZone.setSupplyPlenum((ThermalZone)this.SupplyPlenum.ToOS(model));
            if (this.ReturnPlenum != null) newZone.setReturnPlenum((ThermalZone)this.ReturnPlenum.ToOS(model));

            return newZone;
        }

        public ThermalZone ToOS_NoAirLoop(Model model)
        {
            var newZone = (ThermalZone)this.ToOS(model);
            foreach (var item in this.ZoneEquipments)
            {
                var eqp = (ZoneHVACComponent)item.ToOS(model);
                eqp.addToThermalZone(newZone);
            }

            return newZone;
        }

        public override HVACComponent ToOS(Model model)
        {
            //check the model if there's a same named thermal zone
            //if yes, then return it
            //if no, then create a new one

            var optionalNames = this.CustomAttributes.Where(_ => _.Key.FULLNAME == "NAME");
            var optionalZone = new OptionalThermalZone();

            if (optionalNames.Any())
            {
                var zoneName = optionalNames.First().Value.ToString();
                optionalZone = model.getThermalZoneByName(zoneName);
            }

            ThermalZone newZone = null;

            if (optionalZone.is_initialized())
            {
                //get the thermal zone that is generated by HB, 
                //and set the custom attributes.
                newZone = optionalZone.get();

                //Clean the current Ideal air load
                var idealLoads = newZone.equipment().Where(_ => _.OSType() == "OS:ZoneHVAC:IdealLoadsAirSystem");
                if (idealLoads.Any())
                {
                    foreach (var item in idealLoads)
                    {
                        item.remove();
                    }
                }
                newZone.setUseIdealAirLoads(false);

                var aloop = newZone.airLoopHVAC();
                if (aloop.is_initialized())
                {
                    aloop.get().removeBranchForZone(newZone);
                }

                var airT = newZone.airLoopHVACTerminal();
                if (airT.is_initialized())
                {
                    airT.get().remove();
                }
                newZone.SetCustomAttributes(this.CustomAttributes);
            }
            else
            {
                newZone = base.OnNewOpsObj(NewDefaultOpsObj, model);
            }


            //add child to newZone
            this.IB_SizingZone.ToOS(newZone);

          
            return newZone;
        }
        

        public override IB_ModelObject Duplicate()
        {
            
            //Duplicate self;
            var newObj = base.Duplicate(() => new IB_ThermalZone());
            newObj.IsAirTerminalPriorToZoneEquipments = this.IsAirTerminalPriorToZoneEquipments;

            //Duplicate child member; //add new child member to new object;
            newObj.SetAirTerminal((IB_AirTerminal)this.AirTerminal.Duplicate());
            newObj.SetSizingZone((IB_SizingZone)this.IB_SizingZone.Duplicate());


            foreach (var item in this.ZoneEquipments)
            {
                //Duplicate child member; 
                var newItem = (IB_ZoneEquipment)item.Duplicate();

                //add new child member to new object;
                newObj.ZoneEquipments.Add(newItem);
            }

            if (this.SupplyPlenum != null) newObj.SetSupplyPlenum((IB_ThermalZone)this.SupplyPlenum.Duplicate());
            if (this.ReturnPlenum != null) newObj.SetReturnPlenum((IB_ThermalZone)this.ReturnPlenum.Duplicate());

            return newObj;
        }
        
        
        
    }

    public sealed class IB_ThermalZone_FieldSet 
        : IB_FieldSet<IB_ThermalZone_FieldSet, ThermalZone>
    {
        
        private IB_ThermalZone_FieldSet() {}

        public IB_Field Name { get; }
            = new IB_BasicField("Name", "Name")
            {
                DetailedDescription = "A unique identifying name for each coil."
            };


        public IB_Field Multiplier { get; }
            = new IB_BasicField("Multiplier", "Multiplier")
            {
                DetailedDescription = "Zone Multiplier is designed as a “multiplier” for floor area, zone loads, and energy consumed by internal gains. "+
                "It takes the calculated load for the zone and multiplies it, sending the multiplied load to the attached HVAC system. "+
                "The HVAC system size is specified to meet the entire multiplied zone load and will report the amount of the load met in the Zone Air System Sensible Heating or Cooling Energy/Rate output variable. "+
                "Autosizing automatically accounts for multipliers. Metered energy consumption by internal gains objects such as Lights or Electric Equipment will be multiplied. "+
                "The default is 1."
            };

        public IB_Field ZoneInsideConvectionAlgorithm { get; }
            = new IB_BasicField("ZoneInsideConvectionAlgorithm", "InConvection");


        public IB_Field ZoneOutsideConvectionAlgorithm { get; }
            = new IB_BasicField("ZoneOutsideConvectionAlgorithm", "OutConvection");
        


    }
}
