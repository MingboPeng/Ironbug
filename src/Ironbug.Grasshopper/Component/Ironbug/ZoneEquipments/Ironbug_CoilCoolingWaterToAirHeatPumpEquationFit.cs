using System;
using Grasshopper.Kernel;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_CoilCoolingWaterToAirHeatPumpEquationFit : Ironbug_DuplicatableHVACComponent
    {

        public Ironbug_CoilCoolingWaterToAirHeatPumpEquationFit()
          : base("Ironbug_CoilCoolingWaterToAirHeatPumpEquationFit", "CoilHtn_WaterToAir",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(HVAC.IB_CoilCoolingWaterToAirHeatPumpEquationFit_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quinary | GH_Exposure.obscure;

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilCoolingWaterToAirHeatPumpEquationFit", "Coil", "Connect to ZoneHVACWaterToAirHeatPump", GH_ParamAccess.item);
            pManager.AddGenericParameter("WaterSide", "ToWaterLoop", "Connect to chilled water loop's demand side via plantBranches", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_CoilCoolingWaterToAirHeatPumpEquationFit();


            this.SetObjParamsTo(obj);
            var objs = this.SetObjDupParamsTo(obj);
            DA.SetDataList(0, objs);
            DA.SetDataList(1, objs);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.Coil_CoolingWAFit;

        public override Guid ComponentGuid => new Guid("27991F47-609A-4CA7-BBAA-58EB927E49DC");

    }
    
}