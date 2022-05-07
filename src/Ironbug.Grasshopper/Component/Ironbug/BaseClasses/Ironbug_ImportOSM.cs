using System;
using System.IO;
using Grasshopper.Kernel;
using System.Linq;
using System.Collections.Generic;
using Rhino.Geometry;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ImportOSM : Ironbug_Component
    {
        public Ironbug_ImportOSM()
          : base("IB_ImportOSM", "ImportOSM(WIP)",
              "Description",
              "Ironbug", "00:Ironbug")
        {
        }
        protected override System.Drawing.Bitmap Icon => Properties.Resources.importOSM;

        public override Guid ComponentGuid => new Guid("4BAD4D46-8CCC-41E9-B32A-9D3EAF7F71C2");

        public override GH_Exposure Exposure => GH_Exposure.primary;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("OpenStudioFilePath", "_OsmFile", "OpenStudioFilePath", GH_ParamAccess.item);
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("OsZones", "OsZones", "Connect to Ironbug_ThermalZones", GH_ParamAccess.list);
            //pManager.AddTextParameter("OsZoneNames", "Names", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("OsAirloops", "OsAirloops", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("OsPlantloops", "OsPlantloops", "", GH_ParamAccess.list);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var file = string.Empty;
            if (!DA.GetData(0, ref file)) return;

            if (File.Exists(file) && Path.GetExtension(file).ToLower() == ".osm")
            {
                var m = Model_Extensions.LoadOpsModel(file);
                var zs = m.getThermalZones();
                var names = zs.Select(_ => _.nameString()).ToList();
                names.Sort();

                
                var osZones = zs.AsParallel().Select(_ => _.ToOsZone(out List<Brep> glzs));
                //DA.SetDataList(0, oszs);
                DA.SetDataList(0, osZones);

                var airlps = m.getAirLoopHVACs().Select(_ => _.nameString()).ToList();
                airlps.Sort();

                DA.SetDataList(1, airlps.Select(_ => new HVAC.BaseClass.IB_ExistingObj(_, file)));

                var plantlps = m.getPlantLoops().Select(_ => _.nameString()).ToList();
                plantlps.Sort();
                DA.SetDataList(2, plantlps.Select(_ => new HVAC.BaseClass.IB_ExistingObj(_, file)));

            }
            
        }

        
    }
}