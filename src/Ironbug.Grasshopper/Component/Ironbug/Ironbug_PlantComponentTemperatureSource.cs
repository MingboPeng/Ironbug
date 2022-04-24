using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_PlantComponentTemperatureSource : Ironbug_DuplicableHVACWithParamComponent
    {
        public Ironbug_PlantComponentTemperatureSource()
          : base("Ironbug_PlantComponentTemperatureSource", "PlantComponent_TSource",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_PlantComponentTemperatureSource_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;


        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PlantComponentTemperatureSource", "PlantComponent", "PlantComponentTemperatureSource for plant loop's supply.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_PlantComponentTemperatureSource();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.TSource;

        public override Guid ComponentGuid => new Guid("{60B74E15-C184-4242-B8DF-39B249831A41}");
    }
}