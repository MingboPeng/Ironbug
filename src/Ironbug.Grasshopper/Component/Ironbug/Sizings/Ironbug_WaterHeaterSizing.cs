using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_WaterHeaterSizing : Ironbug_HVACWithParamComponent
    {
        public Ironbug_WaterHeaterSizing()
          : base("IB_WaterHeaterSizing", "WaterHeaterSizing",
              "Description",
              "Ironbug", "06:Sizing&Controller",
              typeof(HVAC.IB_WaterHeaterSizing_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("WaterHeaterSizing", "Sz", "WaterHeaterSizing", GH_ParamAccess.item);
        }
        
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_WaterHeaterSizing();

            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.WaterHeaterSizing;

        public override Guid ComponentGuid => new Guid("415887A5-5B04-47BA-89BC-4DBCE2113AD8");
    }
}