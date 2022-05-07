using System;
using System.IO;
using Grasshopper.Kernel;
using System.Collections.Generic;

using Ironbug.HVAC;
namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_LoadRefObjects : Ironbug_Component
    {
        public Ironbug_LoadRefObjects()
          : base("IB_LoadRefObjects(WIP)", "LoadRefObject(WIP)",
              "Import the reference objects from osm file. Reference object can only be used for quick setting up HVAC object without specifying its parameters.",
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
            pManager[pManager.AddGenericParameter("Objects", "obj", "Todo..", GH_ParamAccess.item)].DataMapping = GH_DataMapping.Flatten;
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var file = string.Empty;
            if (!DA.GetData(0, ref file)) return;

            if (File.Exists(file) && Path.GetExtension(file).ToLower() == ".osm")
            {

                var m = Model_Extensions.LoadOpsModel(file);

                var tp = string.Empty;
                DA.GetData(1, ref tp); //OS:AirConditioner:VariableRefrigerantFlow
                var iddTp = new OpenStudio.IddObjectType(tp);
               
                var objs = m.getObjectsByType(iddTp);
                
                var osObjs = new List<RefObject>();
                foreach (var item in objs)
                {
                    var osObj = new RefObject(item.nameString(), item.__str__());

                    var obj = item.CastToOsType();
                    
                    if (obj is OpenStudio.ParentObject parentObj)
                    {
                        foreach (var child in parentObj.children())
                        {
                            osObj.AddChild(child.__str__());
                        }
                    }
                    osObjs.Add(osObj);
                }

                DA.SetDataList(0, osObjs);


            }
            
        }

        
    }
}