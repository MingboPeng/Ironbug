using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Ironbug.Grasshopper.Properties;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ThermalZone : Ironbug_HVACComponent
    {

        //private List<IB_ThermalZone> _zones = null;

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
            this.Params.ParameterSourcesChanged += Params_ParameterSourcesChanged;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("HoneybeeZonesOrOsZones", "_HBZonesOrOsZones", "HBZone or OsZones", GH_ParamAccess.list);
            //pManager[0].Optional = true;

            //don't forget to change the names in WatchPuppetStates, when change the name here.
            pManager.AddGenericParameter("AirTerminal", "AirTerminal_", "One air terminal per HBZone, and please provide list of air terminals that matches HBZone amount; \r\nDefault: AirTerminalUncontrolled ", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("ZoneEquipments", "Equipments_", "A list of zone equipments that will be added to each zones.", GH_ParamAccess.list);
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

            var zoneEquipments = new List<IB_ZoneEquipment>();
            DA.GetDataList(2, zoneEquipments);

            IB_SizingZone sizing = null;
            DA.GetData(3, ref sizing);

            var zones = this.CreateZones(HBZones, airTerminals, zoneEquipments, sizing);

            DA.SetDataList(0, zones);
        }
        

        private List<IB_ThermalZone> CreateZones(List<object> HBZonesOrNames, List<IB_AirTerminal> AirTerminals, List<IB_ZoneEquipment> ZoneEquipments, IB_SizingZone Sizing)
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
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input the same amount of air terminals as zones");
                    //return;
                }
            }
            else
            {
                //set the default one
                OSZones.ForEach(_ => _.SetAirTerminal(new IB_AirTerminalSingleDuctUncontrolled()));
            }

            //add ZoneEquipments
            var zoneEquipments = ZoneEquipments;

            if (zoneEquipments.Any())
            {
                
                //more than one zone
                foreach (var eqp in zoneEquipments)
                {
                    //change state
                    var eqpHost = eqp.ToPuppetHost();
                    foreach (var zone in OSZones)
                    {
                        var eqpPuppet = eqpHost.DuplicateAsPuppet() as IB_ZoneEquipment;
                        zone.AddZoneEquipment(eqpPuppet);
                    }
                    eqp.PuppetStateUpdated();
                }
                
            }

            //add Sizing
            var sizing = Sizing != null ? Sizing : new IB_SizingZone();
            
            foreach (var zone in OSZones)
            {
                zone.SetSizingZone(sizing);
                this.SetObjParamsTo(zone);
            }
            
            return OSZones;
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

        private void Params_ParameterSourcesChanged(object sender, GH_ParamServerEventArgs e)
        {
            var isAirTerminalOrZoneEquipment = e.ParameterIndex == 1 || e.ParameterIndex == 2;
            if (e.ParameterSide == GH_ParameterSide.Output || !isAirTerminalOrZoneEquipment) return;

            //only AirTerminal or ZoneEquipment remains
            this.WatchPuppetStates();
        }
        
        private IDictionary<string, IB_ZoneEquipment> hvacComps = new Dictionary<string, IB_ZoneEquipment>();

        //This is really only for trying to cleanup the mass after any input prameter's disconnection
        private void WatchPuppetStates()
        {
            var ATorZE = this.Params.Input.Where(input => input.Name == "AirTerminal" || input.Name == "ZoneEquipments");
            var sources = ATorZE.SelectMany(_ => _.Sources).Where(_ => _.Attributes.GetTopLevel.DocObject is Ironbug_HVACComponent);
            var ibobjs = sources.Select(
                _ =>
                {
                    IB_ZoneEquipment eqp = null;
                    _.VolatileData.AllData(true).First().CastTo(out eqp);
                    return eqp;
                }).Where(obj => obj != null);

            var currentConnectedObjs = ibobjs.ToDictionary(_ => _.GetTrackingID());

            //try to get which one in hvacComps is not in currentConnectedObjs
            var itemNotConnectedAnyMore = hvacComps.Except(currentConnectedObjs);
            foreach (var item in itemNotConnectedAnyMore)
            {
                item.Value.ResetPuppetState();
                item.Value.PuppetStateUpdated();
            }

            //replace hvacComps with currentConnectedObjs
            hvacComps = currentConnectedObjs;
        }
    }
}