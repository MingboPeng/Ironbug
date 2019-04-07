using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.IO;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_SaveOSModel : Ironbug_Component
    {
        protected override System.Drawing.Bitmap Icon => Properties.Resources.saveHVAC; 
        public override Guid ComponentGuid => new Guid("3246f516-d4cf-45e0-b0a7-abb47bb014c1");
        
        public Ironbug_SaveOSModel()
          : base("Ironbug_SaveToFile", "SaveToFile",
              "Description",
              "Ironbug", "HVAC")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("NewFilePath", "path", "New OSM file path. This file will be deleted first if it is existed", GH_ParamAccess.item);
            pManager.AddGenericParameter("AirLoops", "AirLoops", "Zone with HVAC system set", GH_ParamAccess.list);
            pManager.AddGenericParameter("PlantLoops", "PlantLoops", "PlantLoops", GH_ParamAccess.list);
            pManager.AddGenericParameter("VRFSystems", "VRFSystems", "VRFSystems", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Write", "_write", "Write the OpenStudio file.", GH_ParamAccess.item, false);

            pManager[0].Optional = true;
            pManager[1].Optional = true;
            pManager[1].DataMapping = GH_DataMapping.Flatten;
            pManager[2].Optional = true;
            pManager[2].DataMapping = GH_DataMapping.Flatten;
            pManager[3].Optional = true;
            pManager[3].DataMapping = GH_DataMapping.Flatten;

        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FilePath", "path", "file path", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string filepath = string.Empty;
            //filepath = @"C:\Users\mingo\Documents\GitHub\Ironbug\doc\osmFile\savedFromGH.osm";
            var airLoops = new List<HVAC.IB_AirLoopHVAC>();
            var plantLoops = new List<HVAC.IB_PlantLoop>();
            var vrfs = new List<HVAC.IB_AirConditionerVariableRefrigerantFlow>();
            bool write = false;

            //var model = new OpenStudio.Model();

            DA.GetData(0, ref filepath);
            DA.GetDataList(1, airLoops);
            DA.GetDataList(2,  plantLoops);
            DA.GetDataList(3, vrfs);

            DA.GetData(4, ref write);

            if (!write) return;
            
            if (string.IsNullOrEmpty(filepath)) return;
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            var hvac = new HVAC.IB_HVACSystem(airLoops, plantLoops, vrfs);
            var saved = hvac.SaveHVAC(filepath);

            if (saved)
            {
                DA.SetData(0, filepath);
            }
            
            
            
        }



    }
}