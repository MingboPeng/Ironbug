using System;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_HumidifierSteamElectric : Ironbug_DuplicatableHVACWithParamComponent
    {
        public Ironbug_HumidifierSteamElectric()
          : base("Ironbug_HumidifierSteamElectric", "Humidifier",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_HumidifierSteamElectric_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("HumidifierSteamElectric", "Humidifier", "connect to airloop's supply side", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_HumidifierSteamElectric();

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }


        protected override System.Drawing.Bitmap Icon => Resources.Humidifier;

        public override Guid ComponentGuid => new Guid("{821EA278-ECC6-440E-9EDE-9133F24B64F0}");


    }

   
}