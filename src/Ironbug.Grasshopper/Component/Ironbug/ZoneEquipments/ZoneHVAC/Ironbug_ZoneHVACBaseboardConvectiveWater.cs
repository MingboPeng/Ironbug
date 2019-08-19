using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACBaseboardConvectiveWater : Ironbug_HVACWithParamComponent
    {
        
        /// Initializes a new instance of the Ironbug_ZoneHVACUnitHeater class.
        
        public Ironbug_ZoneHVACBaseboardConvectiveWater()
          : base("Ironbug_ZoneHVACBaseboardConvectiveWater", "BaseboardWaterC",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACBaseboardConvectiveWater_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        
        
        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("CoilHeatingWaterBaseboard", "coil_", "Heating coil to provide heating source. Only CoilHeatingWaterBaseboard is accepted.", GH_ParamAccess.item);

        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACBaseboardConvectiveWater", "Baseboard", "Connect to zone's equipment", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ZoneHVACBaseboardConvectiveWater();
            
            
            var coilH = (IB_CoilHeatingWaterBaseboard)null;

            if (DA.GetData(0, ref coilH))
            {
                obj.SetHeatingCoil(coilH);
            }
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon => Properties.Resources.BaseboardWC;

        public override Guid ComponentGuid => new Guid("0849B166-9E6A-4BD2-A743-6BAF790F931F");
    }
}