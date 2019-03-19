using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_WaterUseEquipment : Ironbug_Component
    {
        /// <summary>
        /// Initializes a new instance of the Ironbug_ZoneEquipmentGroup class.
        /// </summary>
        public Ironbug_WaterUseEquipment()
          : base("Ironbug_WaterUseEquipment", "WaterUseEquipment",
              "Description",
               "Ironbug", "02:LoopComponents")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("WaterUseLoad", "load_", "Use Ironbug_WaterUseEquipmentDefinition", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddBrepParameter("HBZone", "HBZone_", "Honeybee Zone", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("WaterUseEquipment", "waterEqp", "Connect to Ironbug_WaterUseConnections", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            HVAC.IB_WaterUseEquipmentDefinition load = new HVAC.IB_WaterUseEquipmentDefinition();
            DA.GetData(0, ref load);
            GH_Brep hbZone = null;
            var spaceName = string.Empty;
            DA.GetData(1, ref hbZone);
            if (hbZone!= null)
            {
                var names = CallFromHBHive(new List<GH_Brep>() { hbZone });
                if (names.Any())
                {
                    spaceName = names.First();
                }
            }

            var obj = new HVAC.IB_WaterUseEquipment(load);
            obj.SetSpace(spaceName);

            DA.SetData(0, obj);

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.WaterUseEquip;

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("3A46B628-A815-4994-A919-5FD10CB87CF1");

        
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
    }



}