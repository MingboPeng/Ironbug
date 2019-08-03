using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_GroundHeatExchangerVertical : Ironbug_DuplicatableHVACComponent
    {
        
        public Ironbug_GroundHeatExchangerVertical()
          : base("Ironbug_GroundHeatExchangerVertical", "GroundHXVertical",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_GroundHeatExchangerVertical_FieldSet))
        {
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.GSHPV;
        public override Guid ComponentGuid => new Guid("9FF66F1C-C6C1-4C81-B12F-333230F2FF42");
        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("GFunctions", "GFunc", "A list of values for gFunctions. This list has to be even count. The odd item has to be gFunctionLN, and its value the next item", GH_ParamAccess.list);
            pManager[0].Optional = true;
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("GroundHeatExchangerVertical", "GroundHX", "GroundHeatExchangerVertical", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_GroundHeatExchangerVertical();
            var gfuncs = new List<string>();
            if (DA.GetDataList(0, gfuncs))
            {
                obj.AddGFuncs(gfuncs);
            }
            
            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }



    }

   
}