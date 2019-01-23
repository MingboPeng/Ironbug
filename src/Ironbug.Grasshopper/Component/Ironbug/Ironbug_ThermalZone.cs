using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
using Ironbug.Grasshopper.Properties;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ThermalZone : Ironbug_HVACComponentBase
    {
        private bool SecondRun = false;
        /// <summary>
        /// Initializes a new instance of the Ironbug_ThermalZone class.
        /// </summary>
        public Ironbug_ThermalZone()
          : base("Ironbug_ThermalZone", "ThermalZone",
              "Description",
              "Ironbug", "00:Ironbug",
              typeof(IB_ThermalZone_DataFieldSet))
        {
            //this.Params.ParameterChanged += Params_ParameterChanged;
            this.Params.ParameterSourcesChanged += Params_ParameterSourcesChanged;
            //this.SolutionExpired += Ironbug_ThermalZone_SolutionExpired;
        }

       

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("HoneybeeZone", "_HBZones", "HBZone", GH_ParamAccess.list);
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

        protected override void BeforeSolveInstance()
        {
            var doc = OnPingDocument();
            
            if (!this.SecondRun)
            {
                //var p = doc.SolutionProgress(out i, out mi);
                doc.SolutionEnd += Doc_SolutionEnd;
                doc?.RequestAbortSolution();
            }

            this.SecondRun = false;
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            DA.SetDataList(0, this._zones);
            this.SecondRun = false;

        }

        private void Doc_SolutionEnd(object sender, GH_SolutionEventArgs e)
        {
            this.OnPingDocument().SolutionEnd -= Doc_SolutionEnd;


            this.CreateZones();

            this.SecondRun = true;
            var outp = this.Params.Output[0];
            
            this.ExpireSolution(false);

            this.OnPingDocument().NewSolution(false);

        }
        

        private void CreateZones()
        {
            var willNeedSecondRun = false;
            var HBZones = new List<GH_Brep>();
            var OSZones = new List<IB_ThermalZone>();

            var zoneNames = new List<string>();
            var hbZoneIn = this.Params.Input[0];
            if (hbZoneIn == null) return;

            var phase = hbZoneIn.Phase;
            hbZoneIn.CollectData();
            HBZones = hbZoneIn.VolatileData.AllData(true).Select(_ => (_ as GH_Brep)).ToList();
          
                var hbzones = HBZones.SkipWhile(_ => _ is null);
                zoneNames = CallFromHBHive(hbzones).ToList();
            

            foreach (var name in zoneNames)
            {
                OSZones.Add(new IB_ThermalZone(name));
            }

            if (!OSZones.Any())
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No valid HBZones!");
                return;
            } 
            //add airTerminal
            var airTerminals = new List<IB_AirTerminal>();
            this.Params.Input[1].CollectData();
            airTerminals = this.Params.Input[1].VolatileData.AllData(true).Select(_ => (_ as GH_ObjectWrapper).Value as IB_AirTerminal).ToList();
            if (airTerminals.Any())
            {
                //reset all puppetable state first, as it might came from previous solution.
                airTerminals.ForEach(_ => _.ResetPuppetState());
                //tracking all previously connected objs

                if (airTerminals.Count != OSZones.Count && airTerminals.Count == 1)
                {
                    //change state
                    var airTerminal = airTerminals.First().ToPuppetHost();
                    foreach (var zone in OSZones)
                    {
                        var puppet = airTerminal.DuplicateAsPuppet() as IB_AirTerminal;
                        zone.SetAirTerminal(puppet);
                    }
                    airTerminal.PuppetStateUpdated();

                    //willNeedSecondRun = true;
                    //this.SecondRun = true;

                }
                else if (airTerminals.Count == OSZones.Count)
                {
                    for (int i = 0; i < airTerminals.Count; i++)
                    {
                        OSZones[i].SetAirTerminal(airTerminals[i]);
                    }
                }
                else
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Input the same amount of air terminals as zones");
                    return;
                }
            }
            else
            {
                //set the default one
                OSZones.ForEach(_ => _.SetAirTerminal(new IB_AirTerminalSingleDuctUncontrolled()));
            }

            //add ZoneEquipments
            var zoneEquipments = new List<IB_ZoneEquipment>();
            this.Params.Input[2].CollectData();
            zoneEquipments = this.Params.Input[2].VolatileData.AllData(true).Select(_ => (_ as GH_ObjectWrapper).Value as IB_ZoneEquipment).ToList();

            if (zoneEquipments.Any())
            {

                //zoneEquipments.ForEach(_ => this.WatchPuppetStates(_));
                zoneEquipments.ForEach(_ => _.ResetPuppetState());
                if (OSZones.Count == 1)
                {
                    //only one zone
                    foreach (var eqp in zoneEquipments)
                    {
                        OSZones.First().AddZoneEquipment(eqp);
                    }
                }
                else
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

                    //willNeedSecondRun = true;
                    //this.SecondRun = true;
                }


            }

            //add Sizing
            var sizing = new IB_SizingZone();
            this.Params.Input[3].CollectData();
            var szs = this.Params.Input[3].VolatileData.AllData(true).Select(_ => (_ as GH_ObjectWrapper).Value as IB_SizingZone).ToList();
            if (szs.Any())
            {
                sizing = szs.ElementAt(0);
                OSZones.ForEach(_ => _.SetSizingZone(sizing));
            }


            foreach (var zone in OSZones)
            {
                this.SetObjParamsTo(zone);
            }

            this._zones = OSZones;
            this.SecondRun = willNeedSecondRun;
        }
        private List<IB_ThermalZone> _zones;


        protected override System.Drawing.Bitmap Icon => Resources.ThermalZone;
        
        public override Guid ComponentGuid => new Guid("8aa3ced0-54bb-4cc3-b53b-9b63dbe714a0");

        private static IEnumerable<string> CallFromHBHive(IEnumerable<GH_Brep> inBreps)
        {
            var HBIDs = new List<string>();
            foreach (var item in inBreps)
            {
                
                if (inBreps is null) continue;

                //todo: check if HBID existed
                var HBID = item.Value.UserDictionary["HBID"] as string;
                //string formatedHBID = string.Format("['{0}']['{1}']", HBID[0], HBID[1]);
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

        //private void Params_ParameterChanged(object sender, GH_ParamServerEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        
        IDictionary<string, IB_ZoneEquipment> hvacComps = new Dictionary<string, IB_ZoneEquipment>();

        //This is really only for trying to cleanup the mass after any input prameter's disconnection
        private void WatchPuppetStates()
        {
            var ATorZE = this.Params.Input.Where(input => input.Name == "AirTerminal" || input.Name == "ZoneEquipments");
            var sources = ATorZE.SelectMany(_ => _.Sources).Where(_=>_.Attributes.GetTopLevel.DocObject is Ironbug_HVACComponentBase);
            var ibobjs = sources.Select(
                _ => {
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