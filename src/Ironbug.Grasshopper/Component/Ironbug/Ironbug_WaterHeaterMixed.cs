using System;
using Grasshopper.Kernel;
namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_WaterHeaterMixed : Ironbug_HVACWithParamComponent
    {
        public Ironbug_WaterHeaterMixed()
          : base("IB_WaterHeaterMixed", "WaterHeaterMixed",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_WaterHeaterMixed_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("AmbientTemperatureThermalZone", "AmbientTemperatureThermalZone_", "", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("sizing_", "sizing", "WaterHeaterSizing", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("WaterHeaterMixed", "WaterHeater", "Connect to hot water loop's supply side.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            HVAC.BaseClass.IB_ThermalZone zone = null;
            HVAC.IB_WaterHeaterSizing sizing = null;

            var obj = new HVAC.IB_WaterHeaterMixed();

            if (DA.GetData(0, ref zone)) obj.setAmbientTemperatureThermalZone(zone);
            if (DA.GetData(1, ref sizing)) obj.SetSizing(sizing);


            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }
        
        protected override System.Drawing.Bitmap Icon => Properties.Resources.WaterHeaterMix;

        public override Guid ComponentGuid => new Guid("0CA6FA87-B278-4559-8C5A-D5AA9E667EA6");
    }
}