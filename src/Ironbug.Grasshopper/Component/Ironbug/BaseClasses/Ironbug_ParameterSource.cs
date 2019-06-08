using Grasshopper.Kernel;
using Ironbug.HVAC.BaseClass;
using System;
using System.Collections.Generic;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ParameterSource : Ironbug_Component
    {
        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("{511FFBD4-7AD5-4E78-9482-356328FAD81D}");
        public override GH_Exposure Exposure => GH_Exposure.secondary | GH_Exposure.obscure;

        public Ironbug_ParameterSource()
          : base("Ironbug_ParameterSource", "ParamSource",
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
            pManager.AddGenericParameter("ParamSource", "param", "Parameter source for HVAC component's param_ input.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var sourceStrings = new List<string>();
            if (DA.GetDataList(0, sourceStrings))
            {
                if (sourceStrings.Count>0)
                {
                    var sourceObj = new ParamSource();
                    sourceObj.SourceData = sourceStrings;
                    DA.SetData(0, sourceObj);
                }
         
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "input object is not a valid OpenStudio object");
            }
            
            
        }
        
        
    }

    public class ParamSource
    {
        private List<string> _sourceData;

        public List<string> SourceData
        {
            get { return _sourceData; }
            set { _sourceData = value; }
        }

    }
}