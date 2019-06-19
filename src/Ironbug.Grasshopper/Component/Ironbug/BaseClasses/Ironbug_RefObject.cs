using Grasshopper.Kernel;
using Ironbug.HVAC.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_RefObject : Ironbug_Component
    {
        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("{511FFBD4-7AD5-4E78-9482-356328FAD81D}");
        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        public Ironbug_RefObject()
          : base("Ironbug_RefObject(WIP)", "RefObj",
              "Description",
              "Ironbug", "00:Ironbug")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("OpenStudio Strings", "_strs", "OpenStudio obj raw data strings", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager[pManager.AddGenericParameter("RefObject", "refObj", "Reference object for HVAC component's param_ input.", GH_ParamAccess.item)].DataMapping = GH_DataMapping.Flatten;
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var sourceStrings = new List<string>();
            if (DA.GetDataList(0, sourceStrings))
            {
                if (sourceStrings.Count>0)
                {
                    var idfobjs = sourceStrings
                    .Select(_ => OpenStudio.IdfObject.load(_))
                    .Where(_ => _.is_initialized())
                    .Select(_ => _.get());

                    var mainObj = idfobjs.FirstOrDefault();
                    if (mainObj is null) throw new ArgumentException("Not valid input!");

                    var sourceObj = new RefObject(mainObj.nameString(), mainObj.__str__());
                    var children = idfobjs.Skip(1);
                    foreach (var item in children)
                    {
                        sourceObj.AddChild(item.__str__());
                    }
                    DA.SetData(0, sourceObj);
                }
         
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "input object is not a valid OpenStudio object");
            }
            
            
        }
        
        
    }

}