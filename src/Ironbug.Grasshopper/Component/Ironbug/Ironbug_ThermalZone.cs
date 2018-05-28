using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Ironbug.Grasshopper.Properties;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ThermalZone : Ironbug_HVACComponentBase
    {
        
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
            pManager.AddBrepParameter("HoneybeeZone", "_HBZones", "HBZone", GH_ParamAccess.list);
            //pManager[0].Optional = true;
            pManager.AddGenericParameter("AirTerminal", "AirTerminal_", "One air terminal per HBZone, and please provide list of air terminals that matches HBZone amount; \r\nDefault: AirTerminalUncontrolled ", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("ZoneEquipments", "Equipments_", "A list of zone equipments that will be added to each zones.", GH_ParamAccess.list);
            pManager[2].Optional = true;
            pManager.AddGenericParameter("SizingZone", "Sizing_", "Zone sizing", GH_ParamAccess.item);
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("OpenStudio ThermalZone", "OSZones", "connect to airloop's demand side", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var HBZones = new List<GH_Brep>();
            var OSZones = new List<IB_ThermalZone>();

            var zoneNames = new List<string>();

            if (DA.GetDataList(0, HBZones))
            {
                zoneNames = CallFromHBHive(HBZones).ToList();
            }

            foreach (var name in zoneNames)
            {
                OSZones.Add(new IB_ThermalZone(name));
            }

            //add airTerminal
            var airTerminals = new List<IB_AirTerminal>();

            if (DA.GetDataList(1, airTerminals))
            {
                //reset all puppetable state first, as it might came from previous solution.
                airTerminals.ForEach(_ => _.ResetPuppetState());
                
                if (airTerminals.Count != OSZones.Count && airTerminals.Count == 1)
                {
                    //change state
                    var airTerminal = airTerminals.First().ToPuppetHost();
                    foreach (var zone in OSZones)
                    {
                        var puppet = airTerminal.DuplicateAsPuppet() as IB_AirTerminal;
                        zone.SetAirTerminal(puppet);
                    }
                    
                }
                else if(airTerminals.Count == OSZones.Count)
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
            if (DA.GetDataList(2, zoneEquipments))
            {
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
                    }
                }

                
            }

            //add Sizing
            var sizing = new IB_SizingZone();
            if (DA.GetData(3, ref sizing))
            {
                OSZones.ForEach(_ => _.SetSizingZone(sizing));
            }

           
            foreach (var zone in OSZones)
            {
                this.SetObjParamsTo(zone);
            }
            
            DA.SetDataList(0, OSZones);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return Resources.ThermalZone;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("8aa3ced0-54bb-4cc3-b53b-9b63dbe714a0"); }
        }

        public static IEnumerable<string> CallFromHBHive(List<GH_Brep> inBreps)
        {
            var HBIDs = new List<string>();
            foreach (var item in inBreps)
            {
                //todo: if null
                //todo: check if HBID existed
                var HBID = item.Value.UserDictionary["HBID"] as string;
                //string formatedHBID = string.Format("['{0}']['{1}']", HBID[0], HBID[1]);
                HBIDs.Add(HBID);
            }

            var HBZoneNames = GetHBObjects(HBIDs).Select(_ => _ as string);
            
            return HBZoneNames;

        }



        public static IList<dynamic> GetHBObjects(List<string> HBIDs)
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
    }
}