using System;
using Grasshopper.Kernel;
using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;

namespace Ironbug.Grasshopper.Component
{
    public class Ironbug_ZoneHVACBaseboardConvectiveElectric : Ironbug_HVACComponent
    {
        
        /// Initializes a new instance of the Ironbug_ZoneHVACUnitHeater class.
        
        public Ironbug_ZoneHVACBaseboardConvectiveElectric()
          : base("Ironbug_ZoneHVACBaseboardConvectiveElectric", "BaseboardElecC",
              "Description",
              "Ironbug", "04:ZoneEquipments",
              typeof(IB_ZoneHVACBaseboardConvectiveElectric_FieldSet))
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ZoneHVACBaseboardConvectiveElectric", "Baseboard", "Connect to zone's equipment", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var obj = new HVAC.IB_ZoneHVACBaseboardConvectiveElectric();
            
            this.SetObjParamsTo(obj);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.BaseboardEC;

        public override Guid ComponentGuid => new Guid("F8F6C3D8-4A16-4ADD-A35A-27CE3489A35B");
    }
}