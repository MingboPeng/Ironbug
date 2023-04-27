using GH_IO.Serialization;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Ironbug.Grasshopper.Properties;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ThermalZone : Ironbug_HVACWithParamComponent
    {
      
        protected override System.Drawing.Bitmap Icon => Resources.ThermalZone;

        public override Guid ComponentGuid => new Guid("8aa3ced0-54bb-4cc3-b53b-9b63dbe714a0");

        public override GH_Exposure Exposure => GH_Exposure.primary;

        
        /// Initializes a new instance of the Ironbug_ThermalZone class.
        
        public Ironbug_ThermalZone()
          : base("IB_ThermalZone", "ThermalZone",
              "Use this component to create thermal zones from Honeybee rooms, OsZones, or zone names",
              "Ironbug", "00:Ironbug",
              typeof(IB_ThermalZone_FieldSet))
        {
        }

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("_HBRooms", "_HBRooms", "HBRoom, OsZones, or zone names", GH_ParamAccess.list);

            pManager.AddGenericParameter("AirTerminals_", "AirTerminals_", "One air terminal per HBRoom, and please provide list of air terminals that matches HBRoom amount; \r\nDefault: AirTerminalUncontrolled ", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("ZoneEquipments_", "Equipments_", "A ZoneEquipment or ZoneEquipmentGroup per zone. \nUse Ironbug_ZoneEquipmentGroup if you have more than one equipments for one zone", GH_ParamAccess.list);
            pManager[2].Optional = true;
            pManager.AddGenericParameter("SizingZone_", "Sizing_", "Zone sizing", GH_ParamAccess.item);
            pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("OpenStudio ThermalZone", "Zones", "connect to airloop's demand side", GH_ParamAccess.list);
        }

        

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.Message = string.Empty;
            if (this._allowMultiAirloops)
                this.Message = "Allow Multi-AirLoops";

            var HBZones = new List<object>();
            if (!DA.GetDataList(0, HBZones)) return;
            if (!HBZones.Any()) return;

            var airTerminals = new List<IB_AirTerminal>();
            DA.GetDataList(1, airTerminals);

            IB_SizingZone sizing = null;
            DA.GetData(3, ref sizing);
            
            var zones = this.CreateZones(HBZones, airTerminals, sizing);
            
       
            var zoneEquipsGoo = new List<GH_Goo<object>>();
            DA.GetDataList(2, zoneEquipsGoo);

            var zoneEquipObj = zoneEquipsGoo.Select(_ => _.Value);
            var zoneEquipmentGroups = zoneEquipObj.Where(_ => _ is IB_ZoneEquipmentGroup).Select(_=>_ as IB_ZoneEquipmentGroup);
            var zoneEquipments = zoneEquipObj.Where(_ => _ is IB_ZoneEquipment).Select(_ => _ as IB_ZoneEquipment);


            if (zoneEquipmentGroups.Any())
            {
                this.AddZoneEquipmentGroups(zones, zoneEquipmentGroups.ToList());
            }
            else if (zoneEquipments.Any())
            {
                this.AddZoneEquipments(zones, zoneEquipments.ToList());
            }
  
            DA.SetDataList(0, zones);
        }
        

        private List<IB_ThermalZone> CreateZones(List<object> HBZonesOrNames, List<IB_AirTerminal> AirTerminals, IB_SizingZone sizing)
        {
            var OSZones = new List<IB_ThermalZone>();

            var zoneNames = Helper.GetRoomNames(HBZonesOrNames);


            if (!zoneNames.Any())
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No valid HBZones, or OsZones or text zone names!");
                //return;
            }

            foreach (var name in zoneNames)
            {
                OSZones.Add(new IB_ThermalZone(name));
            }

            //add airTerminal
            var airTerminals = AirTerminals;
            if (airTerminals.Any())
            {
                
                if (airTerminals.Count == OSZones.Count)
                {
                    for (int i = 0; i < airTerminals.Count; i++)
                    {
                        OSZones[i].SetAirTerminal(airTerminals[i]);
                    }
                }
                else
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"One air terminal per zone is needed.\nCurrently you have {OSZones.Count} zones, and {airTerminals.Count}  air terminals");
                    //return;
                }
            }
            else
            {
                //set the default one
                OSZones.ForEach(_ => _.SetAirTerminal(new IB_AirTerminalSingleDuctConstantVolumeNoReheat()));
            }
            
            
            
            foreach (var zone in OSZones)
            {
                //add Sizing
                var sz = sizing != null ? sizing : new IB_SizingZone();

                zone.SetSizingZone((IB_SizingZone)sz);
                zone.IsAirTerminalBeforeZoneEquipments = this._isAirTerminalPriorToZoneEquipments;
                zone.AllowMultiAirLoops = this._allowMultiAirloops;
                this.SetObjParamsTo(zone);
            }
            
            //
            return OSZones;
        }

        private void AddZoneEquipmentGroups(List<IB_ThermalZone> Zones,  List<IB_ZoneEquipmentGroup> ZoneEquipmentGroups)
        {
            var OSZones = Zones;
            var eqpGroups = ZoneEquipmentGroups;
            var groupCount = ZoneEquipmentGroups.Count;

            if (groupCount == OSZones.Count)
            {
                for (int i = 0; i < groupCount; i++)
                {
                    var equips = eqpGroups[i].ZoneEquipments;
                    var zone = OSZones[i];
                    foreach (var equip in equips)
                    {
                        zone.AddZoneEquipment(equip);
                    }

                }
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"One zoneEquipmentGroup or zone equipment per zone is needed.\nCurrently you have {OSZones.Count} zones, and {groupCount} equipment group");
                return;
            }
           
        }
        private void AddZoneEquipments(List<IB_ThermalZone> Zones, List<IB_ZoneEquipment> ZoneEquipments)
        {
            var OSZones = Zones;
            var zEquips = ZoneEquipments;
            var zEquipsCount = ZoneEquipments.Count;

            if (zEquipsCount == OSZones.Count)
            {
                for (int i = 0; i < zEquipsCount; i++)
                {
                    OSZones[i].AddZoneEquipment(zEquips.ElementAt(i));
                }
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"One zoneEquipmentGroup or zone equipment per zone is needed.\nCurrently you have {OSZones.Count} zones, and {zEquipsCount} equipment");
                return;
            }
            
        }

        private bool _allowMultiAirloops = false;
        private bool _isAirTerminalPriorToZoneEquipments = true;
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Prioritize AirTerminal over ZoneEquipments", PrioritizeAirTerminal_Clicked, true, this._isAirTerminalPriorToZoneEquipments)
                .ToolTipText = "This will set the AirTerminal prior to all zone equipments";
            Menu_AppendItem(menu, "Allow to be added to multiple Airloops", AllowMultiAirloops_Clicked, true, this._allowMultiAirloops)
              .ToolTipText = "By default, one ThermalZone can only be added to one Airloop. Enabling this option will allow one ThermalZone to co-exist in multiple Airloops.";
            Menu_AppendSeparator(menu);
            base.AppendAdditionalComponentMenuItems(menu);
        }

        private void PrioritizeAirTerminal_Clicked(object sender, EventArgs e)
        {
            this._isAirTerminalPriorToZoneEquipments = !this._isAirTerminalPriorToZoneEquipments;
            this.ExpireSolution(true);
        }
        private void AllowMultiAirloops_Clicked(object sender, EventArgs e)
        {
            this._allowMultiAirloops = !this._allowMultiAirloops;
            this.ExpireSolution(true);
        }

        public override bool Read(GH_IReader reader)
        {
            if (reader.ItemExists("IsAirTerminalPriorToZoneEquipments"))
                _isAirTerminalPriorToZoneEquipments = reader.GetBoolean("IsAirTerminalPriorToZoneEquipments");

            if (reader.ItemExists(nameof(_allowMultiAirloops)))
                _allowMultiAirloops = reader.GetBoolean(nameof(_allowMultiAirloops));

            return base.Read(reader);
        }

        public override bool Write(GH_IWriter writer)
        {
            writer.SetBoolean("IsAirTerminalPriorToZoneEquipments", _isAirTerminalPriorToZoneEquipments);
            writer.SetBoolean(nameof(_allowMultiAirloops), _allowMultiAirloops);
            return base.Write(writer);
        }

    }
}