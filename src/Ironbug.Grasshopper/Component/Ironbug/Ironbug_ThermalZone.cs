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
    public class Ironbug_ThermalZone : Ironbug_HVACComponent
    {
      
        protected override System.Drawing.Bitmap Icon => Resources.ThermalZone;

        public override Guid ComponentGuid => new Guid("8aa3ced0-54bb-4cc3-b53b-9b63dbe714a0");

        public override GH_Exposure Exposure => GH_Exposure.primary;

        /// <summary>
        /// Initializes a new instance of the Ironbug_ThermalZone class.
        /// </summary>
        public Ironbug_ThermalZone()
          : base("Ironbug_ThermalZone", "ThermalZone",
              "Description",
              "Ironbug", "00:Ironbug",
              typeof(IB_ThermalZone_DataFieldSet))
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HoneybeeZonesOrOsZones", "_HBZones", "HBZone or OsZones", GH_ParamAccess.list);

            pManager.AddGenericParameter("AirTerminal", "AirTerminal_", "One air terminal per HBZone, and please provide list of air terminals that matches HBZone amount; \r\nDefault: AirTerminalUncontrolled ", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("ZoneEquipment", "Equipment_", "A ZoneEquipment or ZoneEquipmentGroup per zones. \nUse Ironbug_ZoneEquipmentGroup if you have more than one equipments for one zone", GH_ParamAccess.list);
            pManager[2].Optional = true;
            pManager.AddGenericParameter("SizingZone", "Sizing_", "Zone sizing", GH_ParamAccess.item);
            pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("OpenStudio ThermalZone", "OSZones", "connect to airloop's demand side", GH_ParamAccess.list);
        }

        

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var HBZones = new List<object>();
            if (!DA.GetDataList(0, HBZones)) return;

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
        

        private List<IB_ThermalZone> CreateZones(List<object> HBZonesOrNames, List<IB_AirTerminal> AirTerminals, IB_SizingZone Sizing)
        {
            var OSZones = new List<IB_ThermalZone>();

            var zoneNames = new List<string>();

            if (HBZonesOrNames[0] is GH_Brep)
            {
                var hbzones = HBZonesOrNames.SkipWhile(_ => _ is null).Select(_=>_ as GH_Brep);
                zoneNames = CallFromHBHive(hbzones).ToList();
            }
            else if (HBZonesOrNames[0] is GH_String)
            {
                zoneNames = HBZonesOrNames.Select(_ => (_ as GH_String).Value ).ToList<string>();
            }
            

            if (!zoneNames.Any())
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No valid HBZones or zone names!");
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
                OSZones.ForEach(_ => _.SetAirTerminal(new IB_AirTerminalSingleDuctUncontrolled()));
            }
            
            
            //add Sizing
            var sizing = Sizing != null ? Sizing : new IB_SizingZone();
            
            foreach (var zone in OSZones)
            {
                zone.SetSizingZone(sizing);
                zone.IsAirTerminalPriorToZoneEquipments = this.IsAirTerminalPriorToZoneEquipments;
                this.SetObjParamsTo(zone);
            }
            
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

        private static IEnumerable<string> CallFromHBHive(IEnumerable<GH_Brep> inBreps)
        {
            var HBIDs = new List<string>();
            foreach (var item in inBreps)
            {
                if (inBreps is null) continue;
                
                item.Value.UserDictionary.TryGetString("HBID", out string HBID);
                if (string.IsNullOrEmpty(HBID)) continue;
               
                HBIDs.Add(HBID);
            }

            if (HBIDs.Any())
            {
                return GetHBObjects(HBIDs).Select(_ => _ as string);
            }
            else
            {
                return new List<string>();
            }
        }

        private static IList<dynamic> GetHBObjects(List<string> HBIDs)
        {
            var pyRun = Rhino.Runtime.PythonScript.Create();
            pyRun.SetVariable("HBIDs", HBIDs.ToArray());
            string pyScript = @"
import scriptcontext as sc;
PyHBObjects=[];
for HBID in HBIDs:
    baseKey, key = HBID.split('#')[0], '#'.join(HBID.split('#')[1:])
    HBZone = sc.sticky['HBHive'][baseKey][key];
    PyHBObjects.append(HBZone.name);
";

            pyRun.ExecuteScript(pyScript);
            var PyHBObjects = pyRun.GetVariable("PyHBObjects") as IList<dynamic>;

            return PyHBObjects;
        }


        private bool IsAirTerminalPriorToZoneEquipments = true;
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Prioritize AirTerminal over ZoneEquipments", PrioritizeAirTerminal, true, this.IsAirTerminalPriorToZoneEquipments)
                .ToolTipText = "This will set the AirTerminal prior to all zone equipments";
            Menu_AppendSeparator(menu);
            base.AppendAdditionalComponentMenuItems(menu);
        }

        private void PrioritizeAirTerminal(object sender, EventArgs e)
        {
            this.IsAirTerminalPriorToZoneEquipments = !this.IsAirTerminalPriorToZoneEquipments;
            this.ExpireSolution(true);
        }


        public override bool Read(GH_IReader reader)
        {
            if (reader.ItemExists("IsAirTerminalPriorToZoneEquipments"))
                IsAirTerminalPriorToZoneEquipments = reader.GetBoolean("IsAirTerminalPriorToZoneEquipments");
            
            return base.Read(reader);
        }
        public override bool Write(GH_IWriter writer)
        {
            writer.SetBoolean("IsAirTerminalPriorToZoneEquipments", IsAirTerminalPriorToZoneEquipments);
            return base.Write(writer);
        }

    }
}