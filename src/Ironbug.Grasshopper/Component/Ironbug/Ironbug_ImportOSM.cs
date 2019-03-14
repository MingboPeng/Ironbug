using System;
using System.IO;
using Grasshopper.Kernel;
using System.Linq;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ImportOSM : Ironbug_Component
    {
        public Ironbug_ImportOSM()
          : base("Ironbug_ImportOSM", "ImportOSM",
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
                var p = OpenStudio.OpenStudioUtilitiesCore.toPath(file);
               
                var ov = OpenStudio.IdfFile.loadVersionOnly(p);
                if (ov.is_initialized())
                {
                    var v = ov.get();
                    
                    //var supportedVv = this.v;

                    var supportedV = "2.5.0";

                    var ifNewerVersion = v.GreaterThan(new OpenStudio.VersionString(supportedV));
                    if (ifNewerVersion)
                    {

                        AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Cannot open a newer version of file ({v.str()}). \r\nThis only supports up to {supportedV}!");
                        return;
                    }
                }
                else
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Failed to open this file, and I don't know why!");
                    return;
                }

                var m = OpenStudio.Model.load(p).get();
                var zs = m.getThermalZones();
                var names = zs.Select(_ => _.nameString()).ToList();
                names.Sort();

                var oszs = names.Select(_ => new OsZone(_));
                
                //DA.SetDataList(0, oszs);
                DA.SetDataList(0, names);

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