using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACBaseboardRadiantConvectiveWater : Ironbug_HVACWithParamComponent
    {
        
        /// Initializes a new instance of the Ironbug_ZoneHVACUnitHeater class.
        
        public Ironbug_ZoneHVACBaseboardRadiantConvectiveWater()
          : base("Ironbug_ZoneHVACBaseboardRadiantConvectiveWater", "BaseboardWaterRadC",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACBaseboardRadiantConvectiveWater_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingWaterBaseboardRadiant", "coil_", "Heating coil to provide heating source. Only CoilHeatingWaterBaseboardRadiant is accepted.", GH_ParamAccess.item);
            //pManager[0].Optional = true;
        }

        
        
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACBaseboardRadiantConvectiveWater", "Baseboard", "Connect to zone's equipment", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ZoneHVACBaseboardRadiantConvectiveWater();
            
            
            var coilH = (IB_CoilHeatingWaterBaseboardRadiant)null;

            if (DA.GetData(0, ref coilH))
            {
                obj.SetHeatingCoil(coilH);
            }
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon => Properties.Resources.BaseboardWRC;
        public override Guid ComponentGuid => new Guid("D2206BDE-F49B-40FB-B4FC-4D5C5663D842");
    }
}