using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Ironbug.Grasshopper.Properties;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_FanSystemModel_Adv : Ironbug_DuplicableHVACWithParamComponent
    {
        
        public Ironbug_FanSystemModel_Adv()
          : base("IB_FanSystemModel+", "FanSysModel+",
              "Description",
              "Ironbug", "02:LoopComponents",
              typeof(HVAC.IB_FanSystemModel_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary | GH_Exposure.obscure;

        
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("speeds", "speeds", EPDoc.FanSystemModel.Field_SetSpeedFlowFractionSpeedElectricPowerFraction + "\n\nExample:\n0.33,0.12\n0.66,0.51\n1.0,1.0", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FanSystemModel", "Fan", "", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var spds = new List<string>();
            DA.GetDataList(0, spds);

            var obj = new HVAC.IB_FanSystemModel();
            obj.SetSpeeds(spds);

            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
        }

        protected override System.Drawing.Bitmap Icon => Resources.FanSysModel;

        public override Guid ComponentGuid => new Guid("{5140633A-F350-4235-828A-5DD0670E4040}");


    }

   
}