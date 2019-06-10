using System;
using System.IO;
using Grasshopper.Kernel;
using System.Linq;
using System.Collections.Generic;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_LoadObjects : Ironbug_Component
    {
        public Ironbug_LoadObjects()
          : base("Ironbug_LoadObjects", "LoadObject",
              "Description",
              "Ironbug", "00:Ironbug")
        {
        }
        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("{67857C3C-6827-484E-98B7-5BAD09ACAE1E}");

        public override GH_Exposure Exposure => GH_Exposure.primary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("OpenStudioFilePath", "_OsmFile", "OpenStudioFilePath", GH_ParamAccess.item);
            pManager.AddTextParameter("ObjectType", "type", "Object type, such as OS:Boiler:HotWater", GH_ParamAccess.item);
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Objects", "obj", "Todo..", GH_ParamAccess.list);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var file = string.Empty;
            if (!DA.GetData(0, ref file)) return;

            if (File.Exists(file) && Path.GetExtension(file).ToLower() == ".osm")
            {
                var p = OpenStudio.OpenStudioUtilitiesCore.toPath(file);
               
                var trans = new OpenStudio.VersionTranslator();
                trans.setAllowNewerVersions(false);
                var tempModel = trans.loadModel(p);
                if (!tempModel.is_initialized())
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Failed to open this file, and I don't know why!");
                    return;
                }

                var tp = string.Empty;
                DA.GetData(1, ref tp);
                var iddTp = new OpenStudio.IddObjectType(tp);
               

                var m = tempModel.get();
                //var objs = m.getObjectsByType(iddTp);
                var objs = m.getBoilerHotWaters();
                var osObjs = new List<OsObject>();
                foreach (var item in objs)
                {
                    var osObj = new OsObject(item.nameString(), item.__str__());
                    if (item is OpenStudio.ParentObject parentObj)
                    {
                        foreach (var child in parentObj.children())
                        {
                            osObj.AddChild(child.__str__());
                        }
                    }
                    osObjs.Add(osObj);
                }
                //var children = ((OpenStudio.ParentObject)objs[0]).children().Select(_ => _.nameString());

                DA.SetDataList(0, osObjs);


            }
            
        }

        
    }
}