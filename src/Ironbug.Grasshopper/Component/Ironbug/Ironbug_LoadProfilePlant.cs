using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_LoadProfilePlant : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_LoadProfilePlant()
          : base("IB_LoadProfilePlant", "LoadProfile",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_LoadProfilePlant_FieldSet))
        {
            
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadProfilePlant", "Load", "LoadProfilePlant as a plant demand component", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_LoadProfilePlant();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("{CD1CC589-18FC-4DA8-8509-0D6761C3B4EA}");


    }

   
}